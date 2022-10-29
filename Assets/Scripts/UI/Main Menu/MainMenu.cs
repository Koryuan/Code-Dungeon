using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour, IPanelUI
{
    private MenuButton currentButton = null;

    [Header("Main References")]
    [SerializeField] private MainMenuUI _UI;
    [SerializeField] private OptionMenu _OptionMenu;

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
        if (!_UI) Debug.LogError($"{name} has no UI references");
        if (!playerActionMap) Debug.LogError($"{name} has no action map references");
        if (!_OptionMenu) Debug.LogError($"{name} has no Option Menu references");
    }
    private void InitializeUI()
    {
        // Defalut Initialize
        _UI.Initialize(MoveCurrentButton);
        _OptionMenu.Initialize();

        // Each button
        _UI.AddPlayButtonListener(StartButton);
        _UI.AddLoadButtonListener(LoadButton);
        _UI.AddOptionButtonListener(OptionButton);
        _UI.AddQuitButtonListener(QuitButton);

        MoveCurrentButton(_UI.PlayButton);
    }
    #endregion

    #region Button Function
    private void StartButton()
    {
        _UI.ActiveGuidePanel();
    }
    private void LoadButton()
    {
        _UI.MainMenuPanel(false);
        SaveLoadMenu.Instance.OpenPanel(this, _UI.LoadButton, false);
    }
    private void OptionButton()
    {
        _UI.MainMenuPanel(false);
        _OptionMenu.OpenPanel(this, _UI.OptionButton);
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
        _UI.MainMenuPanel(true);
        LastUI.Select();
    }

    private void OnInterectInput(InputAction.CallbackContext callback)
    {
        if (_UI.GuidePanelIsActive) SceneLoad.LoadTutorialMap();
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
