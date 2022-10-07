using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseSystem : MonoBehaviour
{
    private bool canControl => GameManager.Instance.CurrentState == GameState.Game_Pause_State;

    public event Action OnClosePanel;

    #region Initialization
    public void Initialization()
    {
        CheckReferences();
    }
    private void CheckReferences()
    {
    }
    #endregion

    public void OpenPanel()
    {

    }
    private void ClosePanel(InputAction.CallbackContext Callback)
    {
        if (canControl) OnClosePanel?.Invoke();
    }

    private void OnEnable()
    {
        InputReferences.Instance._PlayerPauseInput.action.performed += ClosePanel;
    }
    private void OnDisable()
    {
        InputReferences.Instance._PlayerPauseInput.action.performed -= ClosePanel;
    }
}