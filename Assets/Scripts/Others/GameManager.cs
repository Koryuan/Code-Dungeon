using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Public References
    public bool CanOpenItemBox => false;
    public bool isInitialize { get; private set; } = false;
    public GameState CurrentState { get; private set; } = GameState.PlayerControl;

    // UI Priority
    private bool dialogboxOpen = false;
    private bool inventoryOpen = false;
    private bool guideOpen = false;
    private bool onEvent = false;

    [Header("Input References")]
    [SerializeField] private InputActionAsset playerActionMap;

    [Header("System References")]
    [SerializeField] private DialogBox dialogSystem;
    [SerializeField] private GuidePanel guideSystem;

    [Header("Object After Initialize")]
    [SerializeField] private List<GameObject> objectAfterInit = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        dialogSystem.OnDialogBoxClose += OnDialogBoxClose;
        guideSystem.OnClosePanel += OnGuideClose;

        foreach (GameObject notActiveObject in objectAfterInit) notActiveObject.SetActive(true);
        isInitialize = true;
    }

    #region Guide System
    public void OpenGuide(GuideContent Content)
    {
        CurrentState = GameState.GuidePanelOpen;
        guideOpen = true;
        guideSystem.OpenGuide(Content);
    }
    private void OnGuideClose()
    {
        guideOpen = false;
        if (dialogboxOpen) CurrentState = GameState.DialogBoxOpen;
        else
        {
            CurrentState = GameState.PlayerControl;
            onEvent = false;
        }
    }
    #endregion

    #region Dialog System
    public void OpenDialogBox(DialogSetting Dialog)
    {
        dialogboxOpen = true;
        dialogSystem.OpenDialog(Dialog);
        CurrentState = GameState.DialogBoxOpen;
    }
    private void OnDialogBoxClose()
    {
        dialogboxOpen = false;
        
        if (guideOpen) CurrentState = GameState.GuidePanelOpen;
        else
        {
            CurrentState = GameState.PlayerControl;
            onEvent = false;
        }
    }
    #endregion

    async public void StartEvent(GameEvent[] EventList)
    {
        foreach(GameEvent gameEvent in EventList)
        {
            await UniTask.WaitUntil(() => onEvent == false);
            if (gameEvent.GuideContent) OpenGuide(gameEvent.GuideContent);
            else if (gameEvent.Dialog) OpenDialogBox(gameEvent.Dialog);
            onEvent = true;
        }
    }

    #region Enable Disable
    private void OnEnable()
    {
        playerActionMap.Enable();
    }

    private void OnDisable()
    {
        playerActionMap.Disable();
    }
    #endregion
}

public enum GameState
{
    PlayerControl,
    DialogBoxOpen,
    GuidePanelOpen,
    InventoryOpen,
    PauseGame,
}