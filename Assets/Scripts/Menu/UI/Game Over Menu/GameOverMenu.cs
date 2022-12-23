using UnityEngine;

public class GameOverMenu : MonoBehaviour, IPanelUI
{
    [SerializeField] private GameOverMenuUI m_ui;

    [Header("Channel")]
    [SerializeField] private MenuManager m_menuManager;
    [SerializeField] private GameOverChannel m_mainChannel;

    #region Initialization
    private void Start()
    {
        CheckReferences();

        m_mainChannel.OnGameOverRequested += GameIsOver;

        m_ui.Initialize();
        m_ui.AddExitButtonListener(ExitButtonFuntion);
        m_ui.AddLoadButtonListener(LoadButtonFunction);
    }
    private void CheckReferences()
    {
        if (!m_menuManager) Debug.LogError("Game Over Menu, has no channel to request state");
    }
    #endregion

    #region Open/Close
    public void GameIsOver() => OpenPanel(null);
    public void OpenPanel(IMenuUI LastUI)
    {
        m_menuManager.OpenMenu(this, null);
        m_ui.OpenPanel(true);

        if (LastUI != null) LastUI.Select();
        else m_ui.LoadButton.Select();

        Debug.Log("Game Over Menu: Opened");
    }
    public void ClosePanel()
    {
        m_menuManager.CloseMenu(this);
        m_ui.OpenPanel(false);
    }
    #endregion

    #region Button Effect
    private void LoadButtonFunction()
    {
        ClosePanel();
        SaveLoadMenu.Instance.OpenPanel(this, m_ui.LoadButton, false);
    }
    private void ExitButtonFuntion()
    {
        ClosePanel();
        SceneLoad.LoadMainMenu();
    }
    #endregion

    private void OnDestroy()
    {
        m_mainChannel.OnGameOverRequested -= GameIsOver;
    }
}