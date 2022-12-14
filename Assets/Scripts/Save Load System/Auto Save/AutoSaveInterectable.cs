using UnityEngine;

public abstract class AutoSaveInterectable : AutoSaveAttach
{
    public abstract void LoadData(bool CanInterect, bool CanPrint, bool CanScan, bool ObjectActivate, bool AnimationActive);
    public virtual void UpdateCanInterect(bool CanInterect)
    {
        Debug.LogWarning("Method not implemented");
    }
    public virtual void UpdateCanPrint(bool CanPrint)
    {
        Debug.LogWarning("Method not implemented");
    }
    public virtual void UpdateCanScan(bool CanScan)
    {
        Debug.LogWarning("Method not implemented");
    }
    public virtual void UpdateObjectActivation(bool Active)
    {
        Debug.LogWarning("Method not implemented");
    }
    public virtual void UpdateAnimationActivation(bool Active)
    {
        Debug.LogWarning("Method not implemeted");
    }
}

[System.Serializable] public class InteractableSaveData : SaveDataAuto
{
    public bool CanInteract;
    public bool CanPrint;
    public bool CanScan;

    public bool ObjectActive;
    public bool AnimationActive;
}