using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxContain : MonoBehaviour
{
    private const float noScrollbarSize = 535f, withScrollbarSize = 515f;

    [SerializeField] private Item item;

    [Header("Button")]
    [SerializeField] private ItemButton itemButton;
    [SerializeField] private RectTransform buttonTransform;

    public ItemButton ButtonUI => itemButton;
    public string ItemDescription => item.ItemDescription;
    public string ItemName => item.ItemName;
    public bool ItemUseable => item.IsUseable;

    public void Initialize(Item Item, Action<int> HoverAction)
    {
        item = Item;
        itemButton.ItemName.text = item.ItemName;
        //itemButton.Button.OnPointerHover += HoverAction;
    }

    public void ChangeSelection(bool IsMe)
    {
        itemButton.Border.enabled = IsMe;
    }
    public void ScrollbarPopOut(bool IsPopOut)
    {
        buttonTransform.sizeDelta = new Vector2((IsPopOut ? withScrollbarSize : noScrollbarSize),65);
        itemButton.ItemName.margin = IsPopOut ? new Vector4(-5,0,30,0) : new Vector4(0,0,0,0);
    }
}

[Serializable] public class ItemButton
{
    public TMP_Text ItemName;
    public Image Border;
    public HoverButton Button;
}