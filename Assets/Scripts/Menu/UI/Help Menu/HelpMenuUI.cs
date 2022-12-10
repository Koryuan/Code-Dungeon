using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] public class HelpMenuUI
{
    [Header("Holder")]
    [SerializeField] private GameObject m_panel;
    [SerializeField] private Image m_imageHolder;
    [SerializeField] private TMP_Text m_textHolder;
    [SerializeField] private Transform m_containerTransform;
    [SerializeField] private ScrollbarNew m_scrollbar;

    [Header("Button")]
    [SerializeField] private Button m_nextButton;
    [SerializeField] private Button m_prevButton;

    private int m_currentIndex = -1;
    private HelpSettings m_currentSettings = null;

    public bool Open => m_panel.activeSelf;
    public Transform ContainerTransform => m_containerTransform;

    #region Initialization
    public void Initialize()
    {
        CheckReferences();

        m_nextButton.onClick.AddListener(NextData);
        m_prevButton.onClick.AddListener(PreviousData);
    }
    private void CheckReferences()
    {
        if (!m_panel) Debug.LogError($"Help Menu, Has no panel references");
        if (!m_imageHolder) Debug.LogError($"Help Menu, Has no image holder references");
        if (!m_textHolder) Debug.LogError($"Help Menu, Has no text holder references");
        if (!m_nextButton) Debug.LogError($"Help Menu, Has no next button references");
        if (!m_prevButton) Debug.LogError($"Help Menu, Has no prev button references");
    }
    #endregion

    public void OpenPanel(bool Open) => m_panel.SetActive(Open);

    #region Update
    public void UpdateSetting(HelpSettings NewSettings)
    {
        m_currentSettings = NewSettings;
        m_currentIndex = -1;
        NextData();
    }
    private void UpdateUI(HelpSettings.Setting Setting)
    {
        var data = Setting.Data;
        m_textHolder.text = data.Text;
        m_imageHolder.sprite = data.Image;

        bool canNext = m_currentIndex + 1 < m_currentSettings.Settings.Length - 1;
        bool canPrev = m_currentIndex - 1 > -1;
        m_nextButton.gameObject.SetActive(canNext);
        m_prevButton.gameObject.SetActive(canPrev);
    }
    public void UpdateScrollBar(RectTransform Target) => m_scrollbar.CenterOnItem(Target);
    #endregion

    #region Button
    private void NextData()
    {
        if (!m_currentSettings || m_currentSettings.Settings.Length == 0 || m_currentIndex+1 > m_currentSettings.Settings.Length-1) 
            return;

        m_currentIndex++;
        UpdateUI(m_currentSettings.Settings[m_currentIndex]);
    }
    private void PreviousData()
    {
        if (!m_currentSettings || m_currentSettings.Settings.Length == 0 || m_currentIndex-1 < 0) return;
        
        m_currentIndex--;
        UpdateUI(m_currentSettings.Settings[m_currentIndex]);
    }
    #endregion

    #region On Destory
    public void DestroyConnection()
    {
        m_nextButton.onClick.RemoveListener(NextData);
        m_prevButton.onClick.RemoveListener(PreviousData);
    }
    #endregion
}