﻿using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Public References
    public bool CanOpenItemBox => false;
    public bool IsInitialize { get; private set; } = false;
    public GameState CurrentState { get; private set; } = GameState.Game_Player_State;

    // UI Priority
    private bool dialogboxOpen = false;
    private bool inventoryOpen = false;
    private bool guideOpen = false;
    private bool pauseOpen = false;
    private bool onEvent = false;

    [Header("Input References")]
    [SerializeField] private InputActionAsset playerActionMap;

    [Header("System References")]
    [SerializeField] private DialogBox dialogSystem;
    [SerializeField] private GuidePanel guideSystem;
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private PauseMenu pauseSystem;

    async private void Awake()
    {
        Instance = this;

        dialogSystem.OnDialogBoxClose += OnDialogBoxClose;
        guideSystem.OnClosePanel += OnGuideClose;

        // Pause System
        pauseSystem.Initialize();
        pauseSystem.OnClosePanel += OnPauseMenuClose;

        // Inventory System
        inventorySystem.Initialize();
        inventorySystem.OnOpenInventory += OnOpenInventory;
        inventorySystem.OnCloseInventory += OnCloseInventory;

        await UniTask.WaitUntil(() => LoadSceneObject.Instance.AllLoad == true);

        IsInitialize = true;
    }

    #region Guide System
    public void OpenGuide(GuideContent Content)
    {
        CurrentState = GameState.Game_Guide_State;
        guideOpen = true;
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
        dialogboxOpen = true;
        dialogSystem.OpenDialog(Dialog);
        CurrentState = GameState.Game_Dialog_State;
    }
    private void OnDialogBoxClose()
    {
        dialogboxOpen = false;
        UpdateState();
    }
    #endregion

    #region Inventory System
    private void OnOpenInventory()
    {
        inventoryOpen = true;
        CurrentState = GameState.Game_Inventory_State;
    }
    private void OnCloseInventory()
    {
        inventoryOpen = false;
        UpdateState();
    }
    #endregion

    #region Pause System
    public void OpenPauseMenu()
    {
        CurrentState = GameState.Game_Pause_State;
        pauseSystem.OpenPanel(null);
        pauseOpen = true;
    }
    private void OnPauseMenuClose()
    {
        pauseOpen = false;
        UpdateState();
    }
    #endregion

    private void UpdateState()
    {
        if (dialogboxOpen) CurrentState = GameState.Game_Dialog_State;
        else if (guideOpen) CurrentState = GameState.Game_Guide_State;
        else if (inventoryOpen) CurrentState = GameState.Game_Inventory_State;
        else if (pauseOpen) CurrentState = GameState.Game_Pause_State;
        else
        {
            CurrentState = GameState.Game_Player_State;
            onEvent = false;
        }
    }

    async public UniTask StartEvent(GameEvent[] EventList)
    {
        foreach(GameEvent gameEvent in EventList)
        {
            await UniTask.WaitUntil(() => onEvent == false);
            if (gameEvent.GuideContent) OpenGuide(gameEvent.GuideContent);
            else if (gameEvent.Dialog) OpenDialogBox(gameEvent.Dialog);
            else if (gameEvent.HasEvent) gameEvent.ActiveAction();

            if (!gameEvent.HasEvent) onEvent = true;
        }
        await UniTask.WaitUntil(() => onEvent == false);
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
    Game_Player_State,
    Game_Dialog_State,
    Game_Guide_State,
    Game_Inventory_State,
    Game_Pause_State,
}