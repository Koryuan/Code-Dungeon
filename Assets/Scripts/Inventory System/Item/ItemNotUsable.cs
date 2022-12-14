using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item/Not Useable")]
public class ItemNotUsable : Item
{
    public override bool Use()
    {
        throw new System.NotImplementedException();
    }
}