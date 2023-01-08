using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Manager/Menu")]
public class MenuManager: ScriptableObject
{
    public delegate void MenuStateCallBack(MenuState NewState);
    public event MenuStateCallBack OnMenuStateChanged;

    [SerializeField] private GameStateChannel m_gameStateChannel;

    private class Menu
    {
        readonly IPanelUI m_panel;
        public IMenuUI LastUI;

        public Menu(IPanelUI NewPanel) => m_panel = NewPanel;

        public IPanelUI Panel => m_panel;
        public void OpenPanel() => m_panel.OpenPanel(LastUI);
    }

    // Currently Open Menu
    private List<Menu> m_menuList = new List<Menu>();
    private MenuState m_currentState = MenuState.None;
    private Menu m_currentMenu => m_menuList[m_menuList.Count - 1];

    #region Open Close
    private Menu FindMenu(IPanelUI SearchedPanel) => m_menuList.Find(x => x.Panel == SearchedPanel);
    public void OpenMenu(IPanelUI OpenedMenu, IMenuUI LastUI)
    {
        if (m_gameStateChannel) m_gameStateChannel.RaiseGameStateRequestChange(GameState.Game_Open_Menu);

        if (m_menuList.Count > 0) m_currentMenu.LastUI = LastUI;
;
        if (FindMenu(OpenedMenu) == null)
        {
            Menu NewMenu = new Menu(OpenedMenu);
            m_menuList.Add(NewMenu);
            //Debug.Log($"Menu Manager: Menu Added, Current Count:{m_menuList.Count}");
        }

        UpdateState(false);
    }
    public void CloseMenu(IPanelUI ClosedMenu)
    {
        m_menuList.Remove(FindMenu(ClosedMenu));
        //Debug.Log("Menu manager: Remove Menu");
        UpdateState(false);
    }
    #endregion

    async private void UpdateState(bool CloseUI)
    {
        if (m_menuList.Count > 0)
        {
            if (m_currentMenu.Panel is CodeMachine) m_currentState = MenuState.CodeMachine;
            else if (m_currentMenu.Panel is CodeMachineMK2) m_currentState = MenuState.CodeMachineMK2;
            else if (m_currentMenu.Panel is HelpMenu) m_currentState = MenuState.Help;
            else if (m_currentMenu.Panel is Inventory) m_currentState = MenuState.Inventory;
            else if (m_currentMenu.Panel is GameOverMenu) m_currentState = MenuState.GameOver;

            if (CloseUI) m_currentMenu.OpenPanel();
        }
        else
        {
            m_currentState = MenuState.None;
            if (m_gameStateChannel) m_gameStateChannel.RaiseGameStateRemoveState(GameState.Game_Open_Menu);
        }

        await UniTask.Delay(100);

        OnMenuStateChanged?.Invoke(m_currentState);
        //Debug.Log($"Menu Manager: Current State -> {m_currentState}");
    }
}

public enum MenuState
{
    None,
    CodeMachine,
    CodeMachineMK2,
    Help,
    Inventory,
    GameOver
}