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

    private int m_currentIndex = 0;
    private int m_totalEvent => m_npcEvent.Count;
    private AutoSaveNPC m_npcAutoSave { get; set; } = null;

    protected override void Awake()
    {
        base.Awake();
        if (m_autoSave is AutoSaveNPC npc)
        {
            m_npcAutoSave = npc;
            m_npcAutoSave.OnDataLoaded += LoadData;
            m_npcAutoSave.LoadData(canInteract, printInteract, scanInteract, gameObject.activeSelf
                , m_interactableAnimator ? m_interactableAnimator.activeSelf : false);
        }
    }
    private void LoadData(SaveDataAuto LoadedSaveData)
    {
        if (LoadedSaveData.New) return;
        if (LoadedSaveData is NPCSaveData oldData)
        {
            // Common data
            canInteract = oldData.CanInteract;
            printInteract = oldData.CanPrint;
            scanInteract = oldData.CanScan;
            gameObject.SetActive(oldData.ObjectActive);
            if (m_interactableAnimator) m_interactableAnimator.SetActive(oldData.AnimationActive);
            m_currentIndex = oldData.CurrentIndex;

            Debug.Log($"{name}, load old save data!");
        }
    }

    async protected override UniTask Interaction()
    {
        if (GameManager.Instance) await GameManager.Instance.StartEvent(m_npcEvent[m_currentIndex].EventList);
    }

    public void UpdateNPCEvent(int NewEvent = 1)
    {
        if (NewEvent > m_totalEvent) return;
        m_currentIndex = NewEvent - 1;
        SaveIndex();
    }
    public void NextEvent()
    {
        if (m_currentIndex + 1 >= m_totalEvent) return;
        m_currentIndex++;
        SaveIndex();
    }
    private void SaveIndex() => m_npcAutoSave?.UpdateCurrentIndex(m_currentIndex);

    public void SaveData() => AutoSaveScene.SaveObjectState(name,m_currentIndex);
}