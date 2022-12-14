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

    protected AutoSaveInterectable m_autoSave;

    public bool CanPrintInterect => printInteract;
    public bool CanScanInterect => scanInteract;
    public bool CanInterect => canInteract;

    protected virtual void Awake()
    {
        m_autoSave = GetComponent<AutoSaveInterectable>();
    }

    #region Normal Interaction
    async public void OnInteract()
    {
        if (canInteract)
        {
            if (disableInteraction) InteractionActivation(false);
            if (disableObjectBefore) ObjectActivation(false);

            try{
                await Interaction();
                if (disableObjectAfter) ObjectActivation(false);
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
            if (disableInteraction) InteractionActivation(false);
            if (disableObjectBefore) ObjectActivation(false);

            await PrintInteraction();

            if (disableObjectAfter) ObjectActivation(false);
        }
    }
    protected virtual UniTask PrintInteraction() => throw new System.NotImplementedException();
    #endregion

    #region Scan Interaction
    async public void OnScanInteract()
    {
        if (scanInteract && canInteract)
        {
            if (disableInteraction) InteractionActivation(false);
            if (disableObjectBefore) ObjectActivation(false);

            await ScanInteraction();

            if (disableObjectAfter) ObjectActivation(false);
        }
    }
    protected virtual UniTask ScanInteraction() => throw new System.NotImplementedException();
    #endregion

    public virtual void ObjectActivation(bool Activate)
    {
        // Auto save
        m_autoSave?.UpdateObjectActivation(Activate);
        AutoSaveScene.SaveObjectState(gameObject.name);

        // Object
        gameObject.SetActive(Activate);
    }
    public virtual void InteractionActivation(bool Activate = false)
    {
        // Auto Save
        m_autoSave?.UpdateCanInterect(Activate);
        AutoSaveScene.SaveObjectState(gameObject.name);

        // Object
        canInteract = Activate;
        if (!Activate) m_interactableAnimator.SetActive(false);
    }
    public virtual void AnimationActivation(bool Activate)
    {
        if (!m_interactableAnimator) return;

        // Auto Save
        m_autoSave?.UpdateAnimationActivation(Activate);

        // Object
        m_interactableAnimator.SetActive(Activate);
    }
    public virtual void PrintActivation(bool Activate)
    {
        m_autoSave?.UpdateCanPrint(Activate);
        printInteract = Activate;
        if (InteractionManager.Instance) InteractionManager.Instance.UpdateState();
    }
    public virtual void ScanActivation(bool Activate)
    {
        m_autoSave?.UpdateCanScan(Activate);
        scanInteract = Activate;
        if (InteractionManager.Instance) InteractionManager.Instance.UpdateState();
    }
}