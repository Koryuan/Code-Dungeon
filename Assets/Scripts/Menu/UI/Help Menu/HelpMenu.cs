using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelpMenu : MonoBehaviour, IPanelUI
{
    [Header("References")]
    [SerializeField] private HelpMenuUI m_ui;
    [SerializeField] private HelpContain m_containPrefab;

    [Header("Channel")]
    [SerializeField] private HelpChannel m_channel;

    private bool hover = true;
    private List<HelpContain> m_contains = new List<HelpContain>();

    #region Initialization
    async private void Start()
    {
        CheckReferences();

        m_ui.Initialize();
        m_channel.OnHelpInserted += AddHelpSetting;

        await UniTask.Delay(100);

        HelpSettings[] LoadedSettings = null;
        while ((LoadedSettings = m_channel.RaiseHelpDataRequested()) == null) await UniTask.Delay(100);
        if (LoadedSettings.Length > 0) LoadHelpSetting(LoadedSettings);
    }
    private void CheckReferences()
    {
        if (m_channel) Debug.LogError("Help Menu, has no channel to get data");
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

    #region Open Close
    public void OpenPanel(IMenuUI LastUI)
    {
        if (m_ui.Open) return;

        MenuManager.Instance.OpenMenu(this);
        m_ui.OpenPanel(true);

        if (m_contains.Count > 0) m_ui.UpdateSetting(m_contains[0].Settings);

        Debug.Log("Help Menu: Opened");
    }
    private void OpenPanel(InputAction.CallbackContext Context) => OpenPanel(null);
    public void ClosePanel(InputAction.CallbackContext Context)
    {
        if (!m_ui.Open) return;

        MenuManager.Instance.CloseMenu(this);
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
    }
}