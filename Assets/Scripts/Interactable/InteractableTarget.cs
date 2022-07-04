using UnityEngine;

public abstract class InteractableTarget : MonoBehaviour 
{
    protected bool canInteract = true;

    [Header("After Interect")]
    [SerializeField] protected bool disabledAI;

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
    protected virtual void Interaction()
    {
        GameManager.Instance.StartEvent(gameEvent);
        if (disabledAI) canInteract = false;
    }
    #endregion

    #region Print Interaction
    public void OnPrintInteract()
    {
        if (printInteract && canInteract) PrintInteraction();
    }
    protected virtual void PrintInteraction()
    {
        Debug.Log("Print Interect");
        if (disabledAI) canInteract = false;
    }
    #endregion

    #region Scan Interaction
    public void OnScanInteract()
    {
        if (scanInteract && canInteract) ;
    }
    protected virtual void ScanInteraction()
    {
        Debug.Log("scan Interect");
        if (disabledAI) canInteract = false;
    }
    #endregion

}