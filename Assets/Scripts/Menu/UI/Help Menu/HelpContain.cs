using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpContain : MonoBehaviour, IMenuUI
{
    [Header("References")]
    [SerializeField] private RectTransform m_containerTransform;
    [SerializeField] private HoverButton m_button;
    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private Image m_border;

    public Action<HelpSettings> OnInterected;
    public HoverButton Button => m_button;
    public HelpSettings Settings { get; private set; }
    public RectTransform ContainerTransform => m_containerTransform;

    public void OnCreation(HelpSettings Settings)
    {
        name = m_nameText.text = Settings.Name;
        this.Settings = Settings;

        CheckReferences();

        SetHighlight(false);

        m_button.onClick.AddListener(OnInterectedCallback);
        m_button.OnSelectEvent += OnSelect;
        m_button.OnDeselectEvent += OnDeselect;
    }
    private void CheckReferences()
    {
        if (!m_containerTransform) Debug.Log($"{name}, has no container transform reference");
        if (!m_button) Debug.Log($"{name}, has no button reference");
        if (!m_nameText) Debug.Log($"{name}, has no text container reference");
        if (!m_border) Debug.Log($"{name}, has no selection border reference");
    }
    private void OnInterectedCallback() => OnInterected?.Invoke(Settings);

    private void OnSelect() => SetHighlight(true);
    private void OnDeselect(bool Select) => SetHighlight(false);
    public void SetHighlight(bool IsHighlighted) => m_border.enabled = IsHighlighted;
    public void Select() => m_button.Select();

    private void OnDestroy()
    {
        m_button.OnSelectEvent -= OnSelect;
        m_button.OnDeselectEvent -= OnDeselect;
        m_button.onClick.RemoveAllListeners();
    }
}