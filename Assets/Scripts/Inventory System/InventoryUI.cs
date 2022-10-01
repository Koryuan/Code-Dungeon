using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private const float onScrollbarViewport = 17;
    private const float onNoScrollbarViewport = 0;

    [Header("General")]
    [SerializeField] private GameObject itemPanel;

    [Header("Text")]
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;

    [Header("Item Container")]
    [SerializeField] private Transform itemContainer;
    [SerializeField] private RectTransform itemContainerViewport;
    [SerializeField] private Scrollbar itemContainerScrollbar;

    [Header("Dialogue Box")]
    [SerializeField] private GameObject dialogTextLeftRight;
    [SerializeField] private GameObject dialogTextCenter;

    [Header("Yes/No Box")]
    [SerializeField] private YesNoBox yesNoPanel;

    public bool isOpen => itemPanel.activeSelf;
    public bool ScrollbarActive => itemContainerScrollbar.isActiveAndEnabled;
    public Transform ItemContainer => itemContainer;
    public YesNoBox _YesNoBox => yesNoPanel;

    private void Awake()
    {
        CheckNullReferences();
    }

    private void CheckNullReferences()
    {
        if (!itemPanel) Debug.LogError($"{name} has no item panel references");
        if (!itemContainer) Debug.LogError($"{name} has no item container references");
    }
    public void UpdateItemInfo(string NewItemName, string NewItemDescription)
    {
        itemName.text = NewItemName;
        itemDescription.text = NewItemDescription;
    }
    public void UpdateViewPort(bool ScrollbarPopOut)
    {
        itemContainerViewport.offsetMax = 
            new Vector2(ScrollbarPopOut ? onScrollbarViewport : onNoScrollbarViewport,itemContainerViewport.offsetMax.y);
    }
    public void OpenPanel() => itemPanel.SetActive(true);
    public void ClosePanel() => itemPanel.SetActive(false);

    public void OnOpenYesNoBox()
    {
        dialogTextLeftRight.SetActive(false);
        dialogTextCenter.SetActive(true);
    }
    public void OnCloseYesNoBox()
    {
        dialogTextLeftRight.SetActive(true);
        dialogTextCenter.SetActive(false);
    }
}