using Cysharp.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SaveLoadMenu : MonoBehaviour
{
    public static SaveLoadMenu Instance;

    [Header("Menu UI")]
    [SerializeField] private SaveLoadMenuUI _UI;

    private IPanelUI previousPanel = null;
    private IMenuUI previousUI = null;

    public event Action OnOpenPanel;
    public event Action OnClosePanel;

    public bool IsInitialize { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
        _UI.Initialization(name);
        IsInitialize = true;
    }

    async public void OpenPanel(IPanelUI PreviousPanel, IMenuUI PreviousUI, bool IsSave)
    {
        previousPanel = PreviousPanel;
        previousUI = PreviousUI;

        _UI.Panel(true);
        _UI.UpdateUIState(IsSave);
        await UniTask.Delay(20);
        OnOpenPanel?.Invoke();
    }

    private void ClosePanel()
    {
        previousPanel?.OpenPanel(previousUI);
        _UI.Panel(false);
        OnClosePanel?.Invoke();
    }
    private void ClosePanel(InputAction.CallbackContext Context)
    {
        if (_UI.IsOpen) ClosePanel();
    }


    async private void OnEnable()
    {
        await UniTask.WaitUntil(() => InputReferences.Instance);
        InputReferences.Instance._MenuCloseInput.action.performed += ClosePanel;
    }
    private void OnDisable()
    {
        InputReferences.Instance._MenuCloseInput.action.performed -= ClosePanel;
    }
}