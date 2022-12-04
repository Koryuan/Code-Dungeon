using Cysharp.Threading.Tasks;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class InteractableTarget : MonoBehaviour 
{
    [SerializeField] protected bool canInteract = true;

    [Header("Before Event")]
    [SerializeField] protected bool disableObjectBefore;

    [Header("After Interect")]
    [SerializeField] protected bool disableObjectAfter;
    [SerializeField] protected bool disableInteraction;

    [Header("Print Scan Interect")]
    [SerializeField] protected bool printInteract;
    [SerializeField] protected bool scanInteract;

    [Header("Animation")]
    [SerializeField] protected GameObject m_interactableAnimator;

    public bool CanPrintInterect => printInteract;
    public bool CanScanInterect => scanInteract;
    public bool CanInterect => canInteract;

    #region Normal Interaction
    async public void OnInteract()
    {
        if (canInteract)
        {
            if (disableInteraction) DisableInteraction();
            if (disableObjectBefore) DisableObject();

            try{
                await Interaction();
                if (disableObjectAfter) DisableObject();
            } catch (System.Exception){
                return;
            }
        }
    }
    protected virtual UniTask Interaction() => throw new System.NotImplementedException();
    #endregion

    #region Print Interaction
    async public void OnPrintInteract()
    {
        if (printInteract && canInteract)
        {
            if (disableInteraction) DisableInteraction();
            if (disableObjectBefore) DisableObject();

            await PrintInteraction();

            if (disableObjectAfter) DisableObject();
        }
    }
    protected virtual UniTask PrintInteraction() => throw new System.NotImplementedException();
    #endregion

    #region Scan Interaction
    async public void OnScanInteract()
    {
        if (scanInteract && canInteract)
        {
            if (disableInteraction) DisableInteraction();
            if (disableObjectBefore) DisableObject();

            await ScanInteraction();

            if (disableObjectAfter) DisableObject();
        }
    }
    protected virtual UniTask ScanInteraction() => throw new System.NotImplementedException();
    #endregion

    protected virtual void DisableObject()
    {
        AutoSaveScene.SaveObjectState(gameObject.name);
        gameObject.SetActive(false);
    }
    protected virtual void DisableInteraction()
    {
        AutoSaveScene.SaveObjectState(gameObject.name); 
        canInteract = false;
        if (m_interactableAnimator) m_interactableAnimator.SetActive(false);
    }
}