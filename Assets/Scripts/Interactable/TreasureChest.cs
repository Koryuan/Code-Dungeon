using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest: InteractableTarget
{
    [Serializable] private struct TreasureChestEvent
    {
        [SerializeField] private string m_name;
        public GameEvent[] Event;
    }
    [Serializable] private class TreasureChestEventList
    {
        [SerializeField] private int m_currentIndex = -1;
        [SerializeField] private List<TreasureChestEvent> m_EventList = new List<TreasureChestEvent>();

        public int CurrentIndex => m_currentIndex;
        public void UpdateIndex(int Index)
        {
            if (Index > m_EventList.Count - 1 || Index < 0) return;
            m_currentIndex = Index;
        }

        public GameEvent[] GetEvent()
        {
            if (m_EventList.Count - 1 < m_currentIndex || m_currentIndex < 0) return null;
            return m_EventList[m_currentIndex].Event;
        }
    }

    [Header("Special Attribute")]
    [SerializeField] private bool isOpen = false;
    [SerializeField] private AudioSource m_audioSource = null;

    [Header("Event Listing")]
    [SerializeField] private TreasureChestEventList m_openEvent;
    [SerializeField] private TreasureChestEventList m_closeEvent;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;

    private AutoSaveTreasureChest m_treasureChestAutoSave { get; set; } = null;
    private GameEvent[] m_EventToPlay => isOpen ? m_openEvent.GetEvent() : m_closeEvent.GetEvent(); 

    protected override void Awake()
    {
        base.Awake();
        if (m_autoSave is AutoSaveTreasureChest treasureAutoSave)
        {
            m_treasureChestAutoSave = treasureAutoSave;
            m_treasureChestAutoSave.AdditionalData(isOpen,m_openEvent.CurrentIndex,m_closeEvent.CurrentIndex);
            m_treasureChestAutoSave.OnDataLoaded += LoadData;
            m_treasureChestAutoSave.LoadData(canInteract, printInteract, scanInteract, gameObject.activeSelf
                , m_interactableAnimator ? m_interactableAnimator.activeSelf : false);
        }
    }
    private void LoadData(SaveDataAuto LoadedSaveData)
    {
        if (LoadedSaveData.New) return;
        if (LoadedSaveData is TreasureChestSaveData oldData)
        {
            // Common data
            canInteract = oldData.CanInteract;
            printInteract = oldData.CanPrint;
            scanInteract = oldData.CanScan;
            gameObject.SetActive(oldData.ObjectActive);
            if (m_interactableAnimator) m_interactableAnimator.SetActive(oldData.AnimationActive);

            // Open update
            isOpen = oldData.Open;
            m_spriteRenderer.sprite = isOpen ? openSprite : closeSprite;

            // Index Load
            m_openEvent.UpdateIndex(oldData.OnOpenIndex);
            m_closeEvent.UpdateIndex(oldData.OnCloseIndex);

            Debug.Log($"{name}, load old treasure chest save data!");
        }
    }

    async protected override UniTask Interaction()
    {
        if (m_EventToPlay == null) return;
        await GameManager.Instance.StartEvent(m_EventToPlay);
    }
    public void ChangeCloseEvent(int Index)
    {
        m_closeEvent.UpdateIndex(Index);
        if (m_treasureChestAutoSave) m_treasureChestAutoSave.UpdateCloseIndex(m_closeEvent.CurrentIndex);
    }
    public void ChangeOpenEvent(int Index)
    {
        m_openEvent.UpdateIndex(Index);
        if (m_treasureChestAutoSave) m_treasureChestAutoSave.UpdateOpenIndex(m_openEvent.CurrentIndex);
    }
    public void OpenTreasureChest(bool Open)
    {
        isOpen = Open;
        m_spriteRenderer.sprite = Open ? openSprite : closeSprite;

        if (m_treasureChestAutoSave) m_treasureChestAutoSave.UpdateOpen(Open);
        if (m_audioSource) AudioManager.Instance.PlayTreasureChestOpen(m_audioSource);
    }
}