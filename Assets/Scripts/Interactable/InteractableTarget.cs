using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class InteractableTarget : MonoBehaviour 
{
    protected bool canInteract = true;

    [Header("After Interect")]
    [SerializeField] protected bool disableObject;
    [SerializeField] protected bool disableInteraction;

    [Header("Print Scan Interect")]
    [SerializeField] protected bool printInteract;
    [SerializeField] protected bool scanInteract;

    [Header("Event")]
    [SerializeField] protected GameEvent[] gameEvent;

    public bool CanPrintInterect => printInteract;
    public bool CanScanInterect => scanInteract;
    public bool CanInterect => canInteract;

    #region Normal Interaction
    public void OnInteract()
    {
        if (canInteract) Interaction();
    }
    async protected virtual void Interaction()
    {
        if (disableInteraction) canInteract = false;
        await GameManager.Instance.StartEvent(gameEvent);
        if (disableObject)
        {
            AutoSaveScene.SaveObjectState(gameObject.name);
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region Print Interaction
    public void OnPrintInteract()
    {
        if (printInteract && canInteract) PrintInteraction();
    }
    protected virtual void PrintInteraction()
    {
        if (disableInteraction) canInteract = false;
        Debug.Log("Print Interect");
        if (disableObject) gameObject.SetActive(false);
    }
    #endregion

    #region Scan Interaction
    public void OnScanInteract()
    {
        if (scanInteract && canInteract) ScanInteraction();
    }
    protected virtual void ScanInteraction()
    {
        if (disableInteraction) canInteract = false;
        Debug.Log("scan Interect");
        if (disableObject) gameObject.SetActive(false);
    }
    #endregion

}