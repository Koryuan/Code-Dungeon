using System;
using UnityEngine;

public class HelpContain : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform m_containerTransform;
    [SerializeField] private HoverButton m_button;

    public Action<HelpSettings> OnInterected;
    public HoverButton Button => m_button;
    public HelpSettings Settings { get; private set; }
    public RectTransform ContainerTransform => m_containerTransform;

    public void OnCreation(HelpSettings Settings)
    {
        name = Settings.Name;
        this.Settings = Settings;

        CheckReferences();

        m_button.onClick.AddListener(OnInterectedCallback);
    }
    private void CheckReferences()
    {
        if (!m_containerTransform) Debug.Log($"{name}, has no container transform reference");
        if (!m_button) Debug.Log($"{name}, has no button reference");
    }
    private void OnInterectedCallback() => OnInterected?.Invoke(Settings);

    private void OnDestroy() => m_button.onClick.RemoveAllListeners();
}