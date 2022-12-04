using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionMenu : MonoBehaviour
{
    private IMenuUI currentUI = null;
    private IPanelUI previousPanel = null;
    private IMenuUI previousUI = null;

    [Header("Main References")]
    [SerializeField] private OptionMenuUI _UI;

    #region Initialization
    private void CheckReferences()
    {
        if (!_UI) Debug.LogError($"{name} has no UI references");
    }
    public void Initialize()
    {
        // Defalut Initialize
        CheckReferences();
        _UI.Initialize(MoveCurrentUI);

        // Each UI
        _UI.AddExitButtonListener(ExitButton);

        // Close since not allow open
        gameObject.SetActive(false);
    }
    #endregion

    #region Open Close
    public void OpenPanel(IPanelUI PreviousPanel, IMenuUI PreviousUI)
    {
        previousPanel = PreviousPanel; previousUI = PreviousUI;

        _UI.OptionMenu(true);
        _UI.SFXSlider.Select(); currentUI = _UI.SFXSlider;
    }
    public void ClosePanel()
    {
        currentUI.SetHighlight(false);
        _UI.OptionMenu(false);
        previousPanel.OpenPanel(previousUI);
    }
    public void ClosePanel(InputAction.CallbackContext Context)
    {
        if (_UI.IsOpen) ClosePanel();
    }
    #endregion

    private void ExitButton() => ClosePanel();

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

    async private void OnEnable()
    {
        await UniTask.WaitUntil(()=> InputReferences.Instance);
        InputReferences.Instance._MenuCloseInput.action.performed += ClosePanel;
    }
    private void OnDisable()
    {
        InputReferences.Instance._MenuCloseInput.action.performed -= ClosePanel;
    }
}