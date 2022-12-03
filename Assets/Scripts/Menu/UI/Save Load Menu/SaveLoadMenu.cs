using System;
using UnityEngine;

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

    public void OpenPanel(IPanelUI PreviousPanel, IMenuUI PreviousUI, bool IsSave)
    {
        previousPanel = PreviousPanel;
        previousUI = PreviousUI;

        _UI.Panel(true);
        _UI.UpdateUIState(IsSave);
        OnOpenPanel?.Invoke();
    }

    private void ClosePanel()
    {
        previousPanel?.OpenPanel(previousUI);
        _UI.Panel(false);
        OnClosePanel?.Invoke();
    }
}