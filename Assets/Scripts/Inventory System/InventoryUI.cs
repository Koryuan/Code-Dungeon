using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private const float onScrollbarViewport = 17;
    private const float onNoScrollbarViewport = 0;

    [Header("General")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("Text")]
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;

    [Header("Item Container")]
    [SerializeField] private Transform itemContainer;
    [SerializeField] private RectTransform itemContainerViewport;
    [SerializeField] private Scrollbar itemContainerScrollbar;

    [Header("Dialogue Box")]
    [SerializeField] private GameObject inventoryText;
    [SerializeField] private GameObject yesNoBoxText;

    [Header("Yes/No Box")]
    [SerializeField] private YesNoBox yesNoPanel;

    public Transform ItemContainer => itemContainer;
    public bool isOpen => inventoryPanel.activeSelf;
    public bool ScrollbarActive => itemContainerScrollbar.isActiveAndEnabled;
    public YesNoBox _YesNoBox => yesNoPanel;

    #region Initialize
    private void Awake()
    {
        CheckNullReferences();
    }
    private void CheckNullReferences()
    {
        if (!inventoryPanel) Debug.LogError($"{name} has no inventory panel references");
        if (!itemContainer) Debug.LogError($"{name} has no item container references");
    }
    #endregion

    public void UpdateItemInfo(string NewItemName, string NewItemDescription)
    {
        itemName.text = NewItemName;
        itemDescription.text = NewItemDescription;
    }
    public void UpdateViewPort(bool ScrollbarPopOut)
        => itemContainerViewport.offsetMax = 
            new Vector2(ScrollbarPopOut ? onScrollbarViewport : onNoScrollbarViewport, itemContainerViewport.offsetMax.y);

    public void InventoryPanel(bool Open) => inventoryPanel.SetActive(Open);
    public void YesNoPanel(bool Open)
    {
        inventoryText.SetActive(!Open);
        yesNoBoxText.SetActive(Open);
    }
}