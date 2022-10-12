using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private MenuButton currentButton = null;
    private bool canControl => GameManager.Instance.CurrentState == GameState.Game_Pause_State;

    [Header("Main References")]
    [SerializeField] private PauseMenuUI _UI;

    public event Action OnClosePanel;

    #region Initialization
    public void Initialize()
    {
        CheckReferences();
        _UI.Initialize(MoveCurrentButton);

        _UI.AddResumeButtonListener(ResumeButton);
        _UI.AddLoadButtonListener(LoadButton);
        _UI.AddOptionButtonListener(OptionButton);
        _UI.AddExitButtonListener(ExitButton);
    }
    private void CheckReferences()
    {
        if (!_UI) Debug.LogError($"{name} has no UI references");
    }
    #endregion

    #region Button Function
    private void ResumeButton() => ClosePanel();
    private void LoadButton() => Debug.Log("Open Load Panel");
    private void OptionButton() => Debug.Log("Open Option Panel");
    private void ExitButton() => SceneLoad.LoadMainMenu();
    #endregion

    private void MoveCurrentButton(MenuButton NewButton)
    {
        if (NewButton)
        {
            if (currentButton) currentButton.ChangeHighlight(false);
            currentButton = NewButton;

            NewButton.ChangeHighlight(true);
            NewButton.Button.Select();
        }
    }
    async public void OpenPanel(MenuButton SelectedButton)
    {
        _UI.PauseMenuPanel(true);
        if (!SelectedButton) _UI.ResumeButton.Button.Select();
        else SelectedButton.Button.Select();

        await UniTask.Delay(10); InputReferences.Instance._PlayerPauseInput.action.performed += ClosePanel;
    }
    private void ClosePanel()
    {
        if (canControl)
        {
            _UI.PauseMenuPanel(false);
            OnClosePanel?.Invoke();
        }
    }
    private void ClosePanel(InputAction.CallbackContext Callback) => ClosePanel();

    #region Enable Disable
    private void OnDisable()
    {
        InputReferences.Instance._PlayerPauseInput.action.performed -= ClosePanel;
    }
    #endregion
}