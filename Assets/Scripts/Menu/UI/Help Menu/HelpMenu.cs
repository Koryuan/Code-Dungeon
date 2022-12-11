﻿using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelpMenu : MonoBehaviour, IPanelUI
{
    [Header("References")]
    [SerializeField] private HelpMenuUI m_ui;
    [SerializeField] private HelpContain m_containPrefab;

    [Header("Channel")]
    [SerializeField] private HelpChannel m_helpChannel;
    [SerializeField] private GameStateChannel m_GameStateChannel;
    [SerializeField] private MenuManager m_menuManager;

    private bool hover = true;
    private List<HelpContain> m_contains = new List<HelpContain>();

    private bool canOpen { get; set; } = true;
    private bool canInterect { get; set; } = false;

    #region Initialization
    async private void Start()
    {
        CheckReferences();

        m_ui.Initialize();
        m_helpChannel.OnHelpInserted += AddHelpSetting;
        m_GameStateChannel.OnGameStateChanged += OnGameStateChanged;
        m_menuManager.OnMenuStateChanged += OnMenuStateChanged;

        await UniTask.Delay(100);

        HelpSettings[] LoadedSettings = null;
        while ((LoadedSettings = m_helpChannel.RaiseHelpListRequested()) == null) await UniTask.Delay(100);
        if (LoadedSettings.Length > 0) LoadHelpSetting(LoadedSettings);
    }
    private void CheckReferences()
    {
        if (m_helpChannel) Debug.LogError("Help Menu, has no channel to get data");
        if (m_containPrefab) Debug.LogError("Help Menu, has no prefab to insert");
    }
    #endregion

    private void AddHelpSetting(HelpSettings NewSettings)
    {
        var Help = Instantiate(m_containPrefab, m_ui.ContainerTransform);
        Help.transform.SetAsFirstSibling();
        Help.OnCreation(NewSettings);

        // Update Action on Item
        Help.Button.OnHoverEvent += () => hover = false;
        Help.Button.OnSelectEvent += () => UpdateScrollbar(Help.ContainerTransform);

        // Add to list
        m_contains.Add(Help);
    }
    private void LoadHelpSetting(HelpSettings[] SavedSettings)
    {
        foreach (HelpSettings settings in SavedSettings) AddHelpSetting(settings);
    }
    private void UpdateScrollbar(RectTransform Target)
    {
        if (hover) m_ui.UpdateScrollBar(Target);
        hover = true;
    }

    private void OnGameStateChanged(GameState NewState) => canOpen = NewState == GameState.Game_Player_State;
    private void OnMenuStateChanged(MenuState NewState) => canInterect = NewState == MenuState.Help;

    #region Open Close
    public void OpenPanel(IMenuUI LastUI)
    {
        if (m_ui.Open || !canOpen) return;

        m_menuManager.OpenMenu(this,null);
        m_ui.OpenPanel(true);

        if (m_contains.Count > 0) m_ui.UpdateSetting(m_contains[0].Settings);

        Debug.Log("Help Menu: Opened");
    }
    private void OpenPanel(InputAction.CallbackContext Context) => OpenPanel(null);
    public void ClosePanel(InputAction.CallbackContext Context)
    {
        if (!m_ui.Open || !canInterect) return;

        m_menuManager.CloseMenu(this);
        m_ui.OpenPanel(false);

        Debug.Log("Help Menu: Closed");
    }
    #endregion

    async private void OnEnable()
    {
        await UniTask.WaitUntil(()=> InputReferences.Instance);
        InputReferences.Instance._Menu_Help.action.performed += OpenPanel;
        InputReferences.Instance._Menu_Help.action.performed += ClosePanel;
    }
    private void OnDisable()
    {
        InputReferences.Instance._Menu_Help.action.performed -= OpenPanel;
        InputReferences.Instance._Menu_Help.action.performed -= ClosePanel;
    }
    private void OnDestroy()
    {
        m_ui.DestroyConnection();
        m_helpChannel.OnHelpInserted -= AddHelpSetting;
        m_GameStateChannel.OnGameStateChanged -= OnGameStateChanged;
        m_menuManager.OnMenuStateChanged -= OnMenuStateChanged;
    }
}