using System.Linq;
using UnityEngine;

public class ObjectNeedItem : MonoBehaviour
{
    [System.Serializable] private struct UseItem
    {
        public bool RemoveOnUse;
        public string ItemName;
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

        Item getItem =  m_itemChannel?.RaiseItemRequested(m_UseItemEvent[EventIndex].ItemName);
        if (getItem != null)
        {
            if (m_UseItemEvent[EventIndex].RemoveOnUse) m_itemChannel.RaiseItemRequestRemove(getItem);
            await GameManager.Instance.StartEvent(m_UseItemEvent[EventIndex].OnFindObject);
        }
        else
        {
            if (m_UseItemEvent[EventIndex].OnNotFindObject == null || m_UseItemEvent[EventIndex].OnNotFindObject.Length == 0) return;
            await GameManager.Instance.StartEvent(m_UseItemEvent[EventIndex].OnNotFindObject);
        }
    }
}