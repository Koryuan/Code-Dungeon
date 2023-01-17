using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Public References
    public bool IsInitialize { get; private set; } = false;
    public GameState CurrentState { get; private set; } = GameState.None;

    // UI Priority
    private bool dialogboxOpen = false;
    private bool guideOpen = false;
    private bool pauseOpen = false;
    private bool saveMenuOpen = false;
    private bool menuOpen = false;
    private bool onEvent = false;

    [Header("Input References")]
    [SerializeField] private InputActionAsset playerActionMap;

    [Header("System References")]
    [SerializeField] private DialogBox dialogSystem;
    [SerializeField] private GuidePanel guideSystem;

    [Header("Object References")]
    [SerializeField] private PlayerController player;

    [Header("Dialog References")]
    [SerializeField] private DialogSetting m_OnHelpAdded;

    [Header("Channel References")]
    [SerializeField] private GameStateChannel m_gameStateChannel;
    [SerializeField] private HelpChannel m_helpChannel;
    [SerializeField] private ItemChannel m_itemChannel;
    [SerializeField] private LoadingChannel m_loadChannel;

    public PlayerController Player => player;

    #region Instance References
    private PauseMenu pause => PauseMenu.Instance;
    private SaveLoadMenu saveLoad => SaveLoadMenu.Instance;
    #endregion

    #region Initialization
    async private void Awake()
    {
        Instance = this;

        CheckReferences();

        dialogSystem.OnDialogBoxClose += OnDialogBoxClose;
        guideSystem.OnClosePanel += OnGuideClose;

        // Game State Channel
        m_gameStateChannel.OnGameStateRequestedChange += UpdateState;
        m_gameStateChannel.OnGameStateRequestedRemove += RemoveState;
        m_gameStateChannel.OnGameEventPassed += StartEventFromChannel;
        m_gameStateChannel.OnGameStateRequested += GetGameState;

        // Other Channel
        m_loadChannel.OnLoadingFinish += StartGame;

        // Pause System
        await UniTask.WaitUntil(() => pause && pause.IsInitialize);
        pause.OnOpenPanel += OnPauseMenuOpen;
        pause.OnClosePanel += OnPauseMenuClose;

        // Save Load Menu
        await UniTask.WaitUntil(() => saveLoad && saveLoad.IsInitialize);
        saveLoad.OnOpenPanel += OnOpenSaveMenu;
        saveLoad.OnClosePanel += OnCloseSaveMenu;

        // Let all object load before play
        await UniTask.WaitUntil(() => LoadSceneObject.Instance.AllLoad == true);

        // Let time Manager move
        await UniTask.WaitUntil(() => TimeManager.Instance != null);

        Debug.Log("Game Manager - Everything loaded");

        await UniTask.WaitUntil(() => AudioManager.Instance != null);
        AudioManager.Instance.PlayBGMInGame();

        IsInitialize = true;
        m_loadChannel.RaiseLoadUpdated(LoadingType.GameManager);
    }
    private void CheckReferences()
    {
        if (!playerActionMap) Debug.LogError($"{name} has no action map for activated");
        if (!dialogSystem) Debug.LogError($"{name} has no dialog box references");
        if (!guideSystem) Debug.LogError($"{name} has no guide system references");
        if (!player) Debug.LogError($"{name} has no player references");
    }
    private void StartGame() => CurrentState = GameState.Game_Player_State;
    #endregion

    #region Guide System
    private void OpenGuide(GuideContent Content)
    {
        CurrentState = GameState.Game_Guide_State;
        guideOpen = true; onEvent = true;
        guideSystem.OpenGuide(Content);
    }
    private void OnGuideClose()
    {
        guideOpen = false;
        UpdateState();
    }
    #endregion

    #region Dialog System
    public void OpenDialogBox(DialogSetting Dialog)
    {
        dialogboxOpen = true; onEvent = true;
        dialogSystem.OpenDialog(Dialog);
        CurrentState = GameState.Game_Dialog_State;
    }
    private void OnDialogBoxClose()
    {
        dialogboxOpen = false;
        UpdateState();
    }
    #endregion

    #region Pause System
    private void OnPauseMenuOpen()
    {
        CurrentState = GameState.Game_Pause_State;
        pauseOpen = true;
    }
    private void OnPauseMenuClose()
    {
        pauseOpen = false;
        UpdateState();
    }
    #endregion

    #region Save Load Menu
    private void OnOpenSaveMenu()
    {
        saveMenuOpen = true;
        CurrentState = GameState.Game_Save_Load_State;
    }
    private void OnCloseSaveMenu()
    {
        saveMenuOpen = false;
        UpdateState();
    }
    #endregion

    private void UpdateState(GameState NewState)
    {
        switch(NewState)
        {
            case GameState.Game_Open_Menu:
                m_gameStateChannel.RaiseGameStateChanged(CurrentState = GameState.Game_Open_Menu);
                menuOpen = true;
                break;
            default:
                RemoveCurrentState();
                UpdateState();
                break;
        }
    }
    private void RemoveState(GameState OldState)
    {
        switch (OldState)
        {
            case GameState.Game_Open_Menu:
                menuOpen = false;
                UpdateState();
                break;
            default:
                RemoveCurrentState();
                UpdateState();
                break;
        }
    }
    private GameState GetGameState() => CurrentState;
    async private void UpdateState()
    {
        bool CheckOpen()
        {
            if (dialogboxOpen)
            {
                CurrentState = GameState.Game_Dialog_State;
                return true;
            }
            else if (guideOpen)
            {
                CurrentState = GameState.Game_Guide_State;
                return true;
            }
            else if (pauseOpen)
            {
                CurrentState = GameState.Game_Pause_State;
                return true;
            }
            else if (saveMenuOpen)
            {
                CurrentState = GameState.Game_Save_Load_State;
                return true;
            }
            else if (menuOpen)
            {
                CurrentState = GameState.Game_Open_Menu;
                return true;
            }
            return false;
        }

        if (!CheckOpen())
        {
            onEvent = false; await UniTask.Delay(10);
            if (onEvent == false && !CheckOpen()) CurrentState = GameState.Game_Player_State;
        }
        m_gameStateChannel.RaiseGameStateChanged(CurrentState);
    }
    private void RemoveCurrentState()
    {
        if (CurrentState == GameState.Game_Open_Menu) menuOpen = false;
    }
    async private void StartEventFromChannel(GameEvent[] EventList)
    {
        await StartEvent(EventList);
    }

    async public UniTask StartEvent(GameEvent[] EventList)
    {
        if (EventList == null) return;
        foreach (GameEvent gameEvent in EventList)
        {
            await UniTask.WaitUntil(() => onEvent == false);

            if (gameEvent.GuideContent) OpenGuide(gameEvent.GuideContent);
            else if (gameEvent.Dialog)
            {
                if (gameEvent.Item)
                {
                    gameEvent.Dialog.AddItemName(gameEvent.Item.ItemName);
                    m_itemChannel.RaiseItemInsert(gameEvent.Item);
                }
                OpenDialogBox(gameEvent.Dialog);
                onEvent = true;
            }
            await UniTask.WaitUntil(() => onEvent == false);

            if (gameEvent.Help) StartHelpEvent(gameEvent.Help);
            await UniTask.WaitUntil(() => onEvent == false);

            if (gameEvent.HasEvent) gameEvent.ActiveAction();
            if (!gameEvent.HasEvent) onEvent = true;
        }
        await UniTask.WaitUntil(() => onEvent == false);
    }
    private void StartHelpEvent(HelpSettings Setting)
    {
        if (m_helpChannel.RaiseHelpSearchRequested(Setting)) return;

        m_helpChannel.RaiseHelpInsert(Setting);

        onEvent = true;
        m_OnHelpAdded.AddHelpname(Setting.Name);

        OpenDialogBox(m_OnHelpAdded);
    }

    #region Enable/Disalbe/Destroy
    private void OnEnable()
    {
        playerActionMap.Enable();
    }
    private void OnDestroy()
    {
        m_gameStateChannel.OnGameEventPassed -= StartEventFromChannel;
        playerActionMap.Disable();
        m_gameStateChannel.OnGameStateRequestedChange -= UpdateState;
        m_gameStateChannel.OnGameStateRequestedRemove -= RemoveState;
        m_gameStateChannel.OnGameStateRequested -= GetGameState;
        m_loadChannel.OnLoadingFinish -= StartGame;
    }
    #endregion
}

public enum GameState
{
    None,
    Game_Player_State,
    Game_Open_Menu,
    Game_Dialog_State,
    Game_Guide_State,
    Game_Pause_State,
    Game_Save_Load_State,
}