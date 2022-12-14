using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channel/Item Channel")]
public class ItemChannel : ScriptableObject
{
    public delegate Item[] ItemsCallback();
    public event ItemsCallback OnItemListRequested;
    public event Action<Item> OnItemInserted;
    public event Func<string, Item> OnItemRequested;
    public event Action<Item> OnItemRemoved;
    public event Action<Item> OnItemRequestRemove;

    public Item[] RaiseItemListRequested()
    {
        if (OnItemListRequested == null) return null;
        return OnItemListRequested?.Invoke();
    }
    public void RaiseItemInsert(Item AddedItem)
    {
        OnItemInserted?.Invoke(AddedItem);
    }
    public Item RaiseItemRequested(string ItemName)
    {
        if (OnItemRequested == null) return null;
        return OnItemRequested?.Invoke(ItemName);
    }
    public void RaiseItemRemoved(Item RemovedItem)
    {
        OnItemRemoved?.Invoke(RemovedItem);
    }
    public void RaiseItemRequestRemove(Item RequestedItem)
    {
        OnItemRequestRemove?.Invoke(RequestedItem);
    }
}