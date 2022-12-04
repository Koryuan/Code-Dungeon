using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour, IPanelUI
{
    private MenuButton currentButton = null;

    [Header("Main References")]
    [SerializeField] private MainMenuUI ui;
    [SerializeField] private OptionMenu _optionMenu;

    [Header("Input References")]
    [SerializeField] private InputActionAsset playerActionMap;

    #region Initialization
    private void Awake()
    {
        CheckReferences();
        InitializeUI();
        playerActionMap.Enable();
    }
    private void CheckReferences()
    {
        if (!ui) Debug.LogError($"{name} has no UI references");
        if (!playerActionMap) Debug.LogError($"{name} has no action map references");
        if (!_optionMenu) Debug.LogError($"{name} has no Option Menu references");
    }
    private void InitializeUI()
    {
        // Defalut Initialize
        ui.Initialize(MoveCurrentButton);
        _optionMenu.Initialize();

        // Each button
        ui.AddPlayButtonListener(StartButton);
        ui.AddLoadButtonListener(LoadButton);
        ui.AddOptionButtonListener(OptionButton);
        ui.AddQuitButtonListener(QuitButton);

        MoveCurrentButton(ui.PlayButton);
    }
    #endregion

    #region Button Function
    private void StartButton()
    {
        ui.ActiveGuidePanel();
    }
    private void LoadButton()
    {
        ui.MainMenuPanel(false);
        SaveLoadMenu.Instance.OpenPanel(this, ui.LoadButton, false);
    }
    private void OptionButton()
    {
        ui.MainMenuPanel(false);
        _optionMenu.OpenPanel(this, ui.OptionButton);
    }
    private void QuitButton()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    private void MoveCurrentButton(MenuButton NewButton)
    {
        if (NewButton)
        {
            if (currentButton) currentButton.SetHighlight(false);
            currentButton = NewButton;

            NewButton.SetHighlight(true);
            NewButton.Button.Select();
        }
    }
    #endregion

    public void OpenPanel(IMenuUI LastUI)
    {
        ui.MainMenuPanel(true);
        LastUI.Select();
    }

    private void OnInterectInput(InputAction.CallbackContext callback)
    {
        if (ui.GuidePanelIsActive) SceneLoad.LoadTutorialMap();
    }

    #region Enable Disable
    private void OnEnable()
    {
        InputReferences.Instance._MenuInterectInput.action.performed += OnInterectInput;
    }

    private void OnDisable()
    {
        InputReferences.Instance._MenuInterectInput.action.performed -= OnInterectInput;
    }
    #endregion
}