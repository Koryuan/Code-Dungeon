using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ObjectNeedItem : MonoBehaviour
{
    [System.Serializable] private struct UseItem
    {
        public string name;
        public bool RemoveOnUse;
        public string[] SearchedList;
        public GameEvent[] OnFindObject;
        public GameEvent[] OnNotFindObject;
    }

    [SerializeField] private ItemChannel m_itemChannel;

    [Header("Event")]
    [SerializeField] private UseItem[] m_UseItemEvent;

    private void Awake()
    {
        if (!m_itemChannel) Debug.LogError($"{name}, forgot to put item channel to request item");
    }

    async public void UseItemOnEvent(int EventIndex)
    {
        if (!m_itemChannel || EventIndex > m_UseItemEvent.Length - 1) return;

        List<Item> itemList = new List<Item>();
        bool itemAvailable = true;

        foreach(string Item in m_UseItemEvent[EventIndex].SearchedList)
        {
            Item getItem = m_itemChannel?.RaiseItemRequested(Item);
            if (!(itemAvailable = getItem)) break;

            itemList.Add(getItem);
        }
        
        if (itemAvailable) 
        {
            if (m_UseItemEvent[EventIndex].RemoveOnUse)
                foreach (Item item in itemList) m_itemChannel.RaiseItemRequestRemove(item);
            await GameManager.Instance.StartEvent(m_UseItemEvent[EventIndex].OnFindObject);
        }
        else
        {
            if (m_UseItemEvent[EventIndex].OnNotFindObject == null || m_UseItemEvent[EventIndex].OnNotFindObject.Length == 0) return;
            await GameManager.Instance.StartEvent(m_UseItemEvent[EventIndex].OnNotFindObject);
        }
    }
}