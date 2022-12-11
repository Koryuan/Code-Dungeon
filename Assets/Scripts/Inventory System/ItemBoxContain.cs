using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxContain : MonoBehaviour, IMenuUI
{
    private const float noScrollbarSize = 535f, withScrollbarSize = 515f;

    private Item item;

    [Header("General")]
    [SerializeField] private RectTransform containerTransform;

    [Header("Button")]
    [SerializeField] private ItemButton itemButton;
    [SerializeField] private RectTransform buttonTransform;

    public Action<ItemBoxContain,Item> OnContainDestroy;

    public HoverButton Button => itemButton.Button;
    public RectTransform ContainerTransform => containerTransform;
    public string ItemDescription => item.ItemDescription;
    public string ItemName => item.ItemName;
    public bool ItemUseable => item.IsUseable;

    #region Intialization
    public void Create(Item Item)
    {
        CheckReferences();

        item = Item;
        name = itemButton.NameText.text = item.ItemName;

        // Button Initialization
        itemButton.Button.OnSelectEvent += OnSelect;
        itemButton.Button.OnDeselectEvent += SetHighlight;

        SetHighlight(false);
    }
    private void CheckReferences()
    {
        itemButton.CheckReferences(name);

        // Transform
        if (!containerTransform) Debug.LogError($"{name} has no <color=red>Container Transform</color> References");
        if (!buttonTransform) Debug.LogError($"{name} has no <color=red>Button Transform</color> References");
    }
    #endregion

    public void Use()
    {
        if (item.Use())
        {
            OnContainDestroy?.Invoke(this, item);
            Destroy(gameObject);
        }
    }

    #region Select/Deselect
    public void OnSelect()
    {
        Select();
        SetHighlight(true);
    }
    public void Select() => itemButton.Button.Select();
    public void SetHighlight(bool IsHighlighted) => itemButton.Border.enabled = IsHighlighted;
    #endregion
    
    public void ScrollbarPopOut(bool IsPopOut)
    {
        buttonTransform.sizeDelta = new Vector2((IsPopOut ? withScrollbarSize : noScrollbarSize), 65);
        itemButton.NameText.margin = IsPopOut ? new Vector4(-5, 0, 30, 0) : new Vector4(0, 0, 0, 0);
    }
}

[Serializable] public class ItemButton
{
    public TMP_Text NameText;
    public Image Border;
    public HoverButton Button;

    public void CheckReferences(string Name)
    {
        if (!NameText) Debug.LogError($"{Name} has no <color=red>name text</color> References");
        if (!Border) Debug.LogError($"{Name} has no <color=red>border</color> References");
        if (!Button) Debug.LogError($"{Name} has no <color=red>button</color> References");
    }
}