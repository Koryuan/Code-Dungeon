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

    public bool CanPrintInterect => printInteract;
    public bool CanScanInterect => scanInteract;
    public bool CanInterect => canInteract;

    #region Normal Interaction
    async public void OnInteract()
    {
        if (canInteract)
        {
            if (disableInteraction) canInteract = false;
            if (disableObjectBefore) DisableObject();

            try{
                await Interaction();
                if (disableObjectAfter) DisableObject();
            } catch (System.Exception){
                return;
            }
        }
    }
    protected abstract UniTask Interaction();
    #endregion

    #region Print Interaction
    async public void OnPrintInteract()
    {
        if (printInteract && canInteract)
        {
            if (disableInteraction) canInteract = false;
            if (disableObjectBefore) DisableObject();

            await PrintInteraction();

            if (disableObjectAfter) DisableObject();
        }
    }
    protected abstract UniTask PrintInteraction();
    #endregion

    #region Scan Interaction
    async public void OnScanInteract()
    {
        if (scanInteract && canInteract)
        {
            if (disableInteraction) canInteract = false;
            if (disableObjectBefore) DisableObject();

            await ScanInteraction();

            if (disableObjectAfter) DisableObject();
        }
    }
    protected abstract UniTask ScanInteraction();
    #endregion

    protected virtual void DisableObject()
    {
        AutoSaveScene.SaveObjectState(gameObject.name);
        gameObject.SetActive(false);
    }
}