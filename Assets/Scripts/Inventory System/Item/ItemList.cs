using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item/Item Listing")]
public class ItemList : ScriptableObject
{
    [SerializeField] private List<ItemCodeUnlocker> m_codeUnlockerList;
    [SerializeField] private List<ItemNotUsable> m_nonUsableList;

    public Item[] GetItemList(string[] IDList)
    {
        List<Item> list = new List<Item>();
        foreach(string ID in IDList)
        {
            if (m_codeUnlockerList.Count > 0 && SearchInCodeUnlockerList(ID, ref list)) continue;
            if (m_nonUsableList.Count > 0 && SearchInNonUsableList(ID, ref list)) continue;
            Debug.LogWarning($"{ID} is not added to list");
        }
        return list.ToArray();
    }
    public bool SearchInCodeUnlockerList(string ID, ref List<Item> ItemList)
    {
        Item searched = m_codeUnlockerList.Find(x => x.ItemName == ID);
        if (!searched) return false;

        ItemList.Add(searched);
        return true;
    }
    public bool SearchInNonUsableList(string ID, ref List<Item> ItemList)
    {
        Item searched = m_nonUsableList.Find(x => x.ItemName == ID);
        if (!searched) return false;

        ItemList.Add(searched);
        return true;
    }
}