using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Public References
    public bool IsInitialize { get; private set; } = false;
    public GameState CurrentState { get; private set; } = GameState.Game_Player_State;

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

    [Header("Channel References")]
    [SerializeField] private GameStateChannel m_gameStateChannel;
    [SerializeField] private HelpChannel m_helpChannel;
    [SerializeField] private ItemChannel m_itemChannel;

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

        IsInitialize = true;
    }
    private void CheckReferences()
    {
        if (!playerActionMap) Debug.LogError($"{name} has no action map for activated");
        if (!dialogSystem) Debug.LogError($"{name} has no dialog box references");
        if (!guideSystem) Debug.LogError($"{name} has no guide system references");
        if (!player) Debug.LogError($"{name} has no player references");
    }
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

    async private void UpdateState()
    {
        if (dialogboxOpen) CurrentState = GameState.Game_Dialog_State;
        else if (guideOpen) CurrentState = GameState.Game_Guide_State;
        else if (pauseOpen) CurrentState = GameState.Game_Pause_State;
        else if (saveMenuOpen) CurrentState = GameState.Game_Save_Load_State;
        else if (menuOpen) CurrentState = GameState.Game_Open_Menu;
        else
        {
            onEvent = false; await UniTask.Delay(10);
            if (onEvent == false) CurrentState = GameState.Game_Player_State;
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
            }
            if (gameEvent.HasEvent) gameEvent.ActiveAction();
            if (!gameEvent.HasEvent) onEvent = true;
        }
        await UniTask.WaitUntil(() => onEvent == false);
    }

    #region Enable/Disalbe/Destroy
    private void OnEnable()
    {
        playerActionMap.Enable();
    }
    private void OnDestroy()
    {
        playerActionMap.Disable();
        m_gameStateChannel.OnGameStateRequestedChange -= UpdateState;
        m_gameStateChannel.OnGameStateRequestedRemove -= RemoveState;
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
    Game_Save_Load_State
}