using UnityEngine;

[CreateAssetMenu(menuName = "Item Setting", order = 100)]
public class Item : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private bool isUseable;
    [SerializeField] [TextArea(10, 50)] private string itemDescription;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public bool IsUseable => isUseable;
}