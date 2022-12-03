using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxContain : MonoBehaviour, IMenuUI
{
    private const float noScrollbarSize = 535f, withScrollbarSize = 515f;

    [Header("General")]
    [SerializeField] private Item item;
    [SerializeField] private RectTransform containerTransform;

    [Header("Button")]
    [SerializeField] private ItemButton itemButton;
    [SerializeField] private RectTransform buttonTransform;

    public Action<ItemBoxContain,Item> OnDestroy;

    public HoverButton Button => itemButton.Button;
    public RectTransform ContainerTransform => containerTransform;
    public string ItemDescription => item.ItemDescription;
    public string ItemName => item.ItemName;
    public bool ItemUseable => item.IsUseable;

    private void Awake()
    {
        if (!containerTransform) Debug.LogError($"{name} has no <color=red>Container Transform</color> References");
        if (!buttonTransform) Debug.LogError($"{name} has no <color=red>Button Transform</color> References");
    }

    public void Initialize(Item Item)
    {
        item = Item;
        itemButton.NameText.text = item.ItemName;
    }

    public void Use()
    {
        if (item.Use())
        {
            OnDestroy?.Invoke(this,item);
            Destroy(gameObject);
        }
    }

    public void Select()
    {
        itemButton.Button.Select();
    }
    public void SetHighlight(bool IsHighlighted) => itemButton.Border.enabled = IsHighlighted;

    public void ScrollbarPopOut(bool IsPopOut)
    {
        buttonTransform.sizeDelta = new Vector2((IsPopOut ? withScrollbarSize : noScrollbarSize),65);
        itemButton.NameText.margin = IsPopOut ? new Vector4(-5,0,30,0) : new Vector4(0,0,0,0);
    }
}

[Serializable] public class ItemButton
{
    public TMP_Text NameText;
    public Image Border;
    public HoverButton Button;
}