using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] public class InventoryUI
{
    private const float onScrollbarViewport = 17;
    private const float onNoScrollbarViewport = 0;

    [Header("General")]
    [SerializeField] private GameObject m_panel;

    [Header("Description")]
    [SerializeField] private TMP_Text m_nameHolder;
    [SerializeField] private TMP_Text m_descriptionHolder;

    [Header("Item Container")]
    [SerializeField] private Transform m_container;
    [SerializeField] private RectTransform m_viewport;
    [SerializeField] private Scrollbar m_scrollbar;

    [Header("Dialog")]
    [SerializeField] private GameObject m_itemDialog;
    [SerializeField] private GameObject m_yesNoDialog;

    [Header("References")]
    [SerializeField] private YesNoBox m_yesNoPanel;
    [SerializeField] private ScrollbarNew m_scrollbarFunction;

    public Transform ItemContainer => m_container;
    public bool ScrollbarActive => m_scrollbar.isActiveAndEnabled;
    public YesNoBox _YesNoBox => m_yesNoPanel;

    #region Initialize
    public void CheckReferences()
    {
        // Panel
        if (!m_panel) Debug.LogError($"Inventory, has no panel references");

        // Description
        if (!m_nameHolder) Debug.LogError("Inventory, has no name holder references");
        if (!m_descriptionHolder) Debug.LogError("Inventory, has no description references");

        // Container
        if (!m_container) Debug.LogError("Inventory, has no container transform references");
        if (!m_viewport) Debug.LogError("Inventory, has no contain viewport transform references");
        if (!m_scrollbar) Debug.LogError("Inventory, has no contain scrollbar references");

        // Dialog
        if (!m_itemDialog) Debug.LogError("Inventory, has no dialog when item references");
        if (!m_yesNoDialog) Debug.LogError("Inventory, has no dialog when yes/no references");

        // Other references
        if (!m_yesNoPanel) Debug.LogError("Inventory, has no yes/no controller references");
        if (!m_scrollbarFunction) Debug.LogError("Inventory, has no scroball for function references");
    }
    #endregion

    public void UpdateItemInfo(ItemBoxContain Contain, bool Hover = false)
    {
        m_nameHolder.text = Contain ? Contain.ItemName : "";
        m_descriptionHolder.text = Contain ? Contain.ItemDescription : "";
        if (Hover) m_scrollbarFunction.CenterOnItem(Contain.ContainerTransform);
    }
    public void UpdateViewPort(bool ScrollbarPopOut)
        => m_viewport.offsetMax = 
            new Vector2(ScrollbarPopOut ? onScrollbarViewport : onNoScrollbarViewport, m_viewport.offsetMax.y);

    public void InventoryPanel(bool Open)
    {
        m_panel.SetActive(Open);
        Update_Dialog(Open);

        if (!Open) _YesNoBox.ClosePanel();
    }
    public void Update_Dialog(bool IsItem)
    {
        m_itemDialog.SetActive(IsItem);
        m_yesNoDialog.SetActive(!IsItem);
    }
}