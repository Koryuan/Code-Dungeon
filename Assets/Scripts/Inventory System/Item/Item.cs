using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("Details")]
    [SerializeField] protected string itemName;
    [SerializeField] protected bool isUseable;
    [SerializeField] [TextArea(10, 50)] protected string itemDescription;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public bool IsUseable => isUseable;

    public abstract bool Use();
}