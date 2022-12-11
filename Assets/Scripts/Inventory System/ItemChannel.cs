using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channel/Item Channel")]
public class ItemChannel : ScriptableObject
{
    public delegate Item[] ItemsCallback();
    public event ItemsCallback OnItemListRequested;
    public event Action<Item> OnItemInserted;
    public event Action<Item> OnItemRemoved;

    public Item[] RaiseItemListRequested()
    {
        if (OnItemListRequested == null) return null;
        return OnItemListRequested?.Invoke();
    }
    public void RaiseItemInsert(Item AddedItem)
    {
        OnItemInserted?.Invoke(AddedItem);
    }
    public void RaiseItemRemove(Item RemovedItem)
    {
        OnItemRemoved?.Invoke(RemovedItem);
    }
}