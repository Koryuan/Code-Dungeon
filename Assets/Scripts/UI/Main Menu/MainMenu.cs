using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    private MainMenuButton currentButton = null;

    [Header("Main References")]
    [SerializeField] private MainMenuUI _UI;
    [SerializeField] private EventSystem _EventSystem;

    [Header("Input References")]
    [SerializeField] private InputActionAsset playerActionMap;

    #region Initialization
    private void Awake()
    {
        CheckReferences();
        InitializeUI();
    }
    private void CheckReferences()
    {
        if (!_UI) Debug.LogError($"{name} has no UI references");
        if (!playerActionMap) Debug.LogError($"{name} has no action map references");
    }
    private void InitializeUI()
    {
        // Defalut Initialize
        _UI.Initialize(MoveCurrentButton);

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
        Debug.Log("Load Game");
    }

    private void OptionButton()
    {
        Debug.Log("Open Option Menu");
    }

    private void QuitButton()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    private void MoveCurrentButton(MainMenuButton NewButton)
    {
        if (NewButton)
        {
            if (currentButton) currentButton.ChangeHighlight(false);
            currentButton = NewButton;

            NewButton.ChangeHighlight(true);
            NewButton.Button.Select();
        }
    }
    #endregion

    private void OnInterectInput(InputAction.CallbackContext callback)
    {
        if (_UI.GuidePanelIsActive) SceneLoadManager.LoadTutorialMap();
    }

    #region Enable Disable
    private void OnEnable()
    {
        playerActionMap.Enable();
        InputReferences.Instance._MenuInterectInput.action.performed += OnInterectInput;
    }

    private void OnDisable()
    {
        playerActionMap.Disable();
        InputReferences.Instance._MenuInterectInput.action.performed -= OnInterectInput;
    }
    #endregion
}
