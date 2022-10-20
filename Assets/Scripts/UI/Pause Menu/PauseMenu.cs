using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour, IPanelUI
{
    private IMenuUI currentUI = null;
    private bool IsOpen { get; set; } = false;
    private bool inControl => GameManager.Instance.CurrentState == GameState.Game_Pause_State && IsOpen;
    private bool canOpen => GameManager.Instance.CurrentState == GameState.Game_Player_State && !IsOpen;

    [Header("Main References")]
    [SerializeField] private PauseMenuUI _UI;
    [SerializeField] private OptionMenu _OptionPanel;

    public static PauseMenu Instance;
    public event Action OnOpenPanel;
    public event Action OnClosePanel;
    public bool IsInitialize { get; private set; } = false;

    #region Initialization
    public void Awake()
    {
        Instance = this;

        CheckReferences();
        _UI.Initialize(MoveCurrentUI);
        _OptionPanel.Initialize();

        _UI.AddResumeButtonListener(ResumeButton);
        _UI.AddLoadButtonListener(LoadButton);
        _UI.AddOptionButtonListener(OptionButton);
        _UI.AddExitButtonListener(ExitButton);

        MoveCurrentUI(_UI.ResumeButton);
        IsInitialize = true;
    }
    private void CheckReferences()
    {
        if (!_UI) Debug.LogError($"{name} has no UI references");
        if (!_OptionPanel) Debug.LogError($"{name} has no Option Panel references");
    }
    #endregion

    #region Button Function
    private void ResumeButton() => ClosePanel();
    private void LoadButton() => Debug.Log("Open Load Panel");
    private void OptionButton()
    {
        _UI.PauseMenuPanel(IsOpen = false);
        _OptionPanel.OpenPanel(this, _UI.OptionButton);
    }
    private void ExitButton() => SceneLoad.LoadMainMenu();
    #endregion

    private void MoveCurrentUI(IMenuUI NewUI)
    {
        if (NewUI != null)
        {
            if (currentUI != null) currentUI.SetHighlight(false);
            currentUI = NewUI;

            NewUI.SetHighlight(true);
            NewUI.Select();
        }
    }

    #region Open Close
    async public void OpenPanel(IMenuUI LastUI)
    {
        if (canOpen)
        {
            OnOpenPanel?.Invoke();
            await UniTask.Delay(10);

            _UI.PauseMenuPanel(IsOpen = true);
            if (LastUI == null) _UI.ResumeButton.Select();
            else LastUI.Select();
        }
    }
    private void ClosePanel()
    {
        if (inControl)
        {
            _UI.PauseMenuPanel(IsOpen = false);
            OnClosePanel?.Invoke();
        }
    }
    #endregion

    #region Input References
    private InputActionReference pauseInput => InputReferences.Instance._PlayerPauseInput;
    private void OpenPanel(InputAction.CallbackContext Callback) => OpenPanel(null);
    private void ClosePanel(InputAction.CallbackContext Callback) => ClosePanel();
    #endregion

    #region Enable Disable
    private void OnEnable()
    {
        pauseInput.action.performed += OpenPanel;
        pauseInput.action.performed += ClosePanel;
    }
    private void OnDisable()
    {
        pauseInput.action.performed -= OpenPanel;
        pauseInput.action.performed -= ClosePanel;
    }
    #endregion
}