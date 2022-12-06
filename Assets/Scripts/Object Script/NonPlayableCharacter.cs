using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter : InteractableTarget
{
    [System.Serializable] private struct NPCEvent
    {
        public GameEvent[] EventList;
    }

    [Header("Special Attribute")]
    [SerializeField] private List<NPCEvent> m_npcEvent = new List<NPCEvent>();

    private int currentEvent = 0;
    private int totalEvent => m_npcEvent.Count;

    async protected override UniTask Interaction()
    {
        if (GameManager.Instance) await GameManager.Instance.StartEvent(m_npcEvent[currentEvent].EventList);
    }

    public void UpdateNPCEvent(int NewEvent = 1)
    {
        if (NewEvent > totalEvent) return;
        currentEvent = NewEvent - 1;
    }
    public void NextEvent()
    {
        Debug.Log("jalan");
        if (currentEvent + 1 >= totalEvent) return;
        Debug.Log("Ke Update");
        currentEvent++;
    }

    public void SaveData() => AutoSaveScene.SaveObjectState(name,currentEvent);
}