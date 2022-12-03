using System.Collections.Generic;
using UnityEngine;

public class MenuManager: MonoBehaviour
{
    #region Instance
    private static MenuManager m_instance;
    public static MenuManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("Menu Manager").AddComponent<MenuManager>();
                DontDestroyOnLoad(m_instance);
            }
            return m_instance;
        }
    }
    #endregion

    private enum MenuState
    {
        None,
        CodeMachine
    }

    // Currently Open Menu
    private MenuState m_currentState = MenuState.None;
    public bool CodeMachineOpen => m_currentState == MenuState.CodeMachine;

    // Menu List
    private List<IPanelUI> m_menuList = new List<IPanelUI>();

    #region Open Close
    public void OpenMenu(IPanelUI OpenedMenu)
    {
        if (GameManager.Instance) GameManager.Instance.OpenMenu();

        m_menuList.Add(OpenedMenu);
        UpdateState(false);
    }
    public void CloseMenu(IPanelUI ClosedMenu)
    {
        m_menuList.Remove(ClosedMenu);
        UpdateState(true);
    }
    #endregion

    private void UpdateState(bool AfterClose)
    {
        if (m_menuList.Count > 0)
        {
            IPanelUI lastUI = m_menuList[m_menuList.Count-1];
            if (lastUI is CodeMachine) m_currentState = MenuState.CodeMachine;

            if (AfterClose) lastUI.OpenPanel(null);
        }
        else
        {
            m_currentState = MenuState.None;
            if (GameManager.Instance) GameManager.Instance.CloseMenu();
        }
    }
}