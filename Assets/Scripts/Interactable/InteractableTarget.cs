using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class InteractableTarget : MonoBehaviour 
{
    protected bool canInteract = true;

    [Header("Before Event")]
    [SerializeField] protected bool disableObjectBefore;

    [Header("After Interect")]
    [SerializeField] protected bool disableObjectAfter;
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
        if (disableObjectBefore) DisableObject();

        await GameManager.Instance.StartEvent(gameEvent);
        if (disableObjectAfter) DisableObject();
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
        if (disableObjectBefore) DisableObject();
        Debug.Log("Print Interect");
        if (disableObjectAfter) DisableObject();
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
        if (disableObjectBefore) DisableObject();
        Debug.Log("scan Interect");
        if (disableObjectAfter) DisableObject();
    }
    #endregion

    protected virtual void DisableObject()
    {
        AutoSaveScene.SaveObjectState(gameObject.name);
        gameObject.SetActive(false);
    }
}