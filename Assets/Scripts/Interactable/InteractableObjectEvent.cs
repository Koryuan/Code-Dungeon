using Cysharp.Threading.Tasks;
using UnityEngine;

public class InteractableObjectEvent : InteractableTarget
{
    [Header("Special Attribute")]
    [SerializeField] protected bool openAtStart = true;
    [SerializeField] protected GameEvent[] eventList;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(openAtStart);

        if (m_autoSave)
        {
            m_autoSave.OnDataLoaded += LoadData;
            m_autoSave?.LoadData(canInteract,printInteract,scanInteract,gameObject.activeSelf
                ,m_interactableAnimator ? m_interactableAnimator.activeSelf : false);
        }
    }

    private void LoadData(SaveDataAuto LoadedData)
    {
        if (LoadedData.New) return;
        if (LoadedData is InteractableSaveData oldData)
        {
            canInteract = oldData.CanInteract;
            printInteract = oldData.CanPrint;
            scanInteract = oldData.CanScan;
            gameObject.SetActive(oldData.ObjectActive);
            if (m_interactableAnimator) m_interactableAnimator.SetActive(oldData.AnimationActive);
        }
    }

    async protected override UniTask Interaction()
    {
        await GameManager.Instance.StartEvent(eventList);
    }

    private void OnDestroy()
    {
        if (m_autoSave) m_autoSave.OnDataLoaded -= LoadData;
    }
}