using Cysharp.Threading.Tasks;
using UnityEngine;

public class AutoSaveObject : AutoSaveInterectable
{
    private InteractableSaveData m_saveData = new InteractableSaveData();

    async public override void LoadData(bool CanInterect, bool CanPrint, bool CanScan, bool ObjectActive, bool AnimationActive)
    {
        m_saveData.ID = name;
        m_saveData.CanInteract = CanInterect;
        m_saveData.CanPrint = CanPrint;
        m_saveData.CanScan = CanScan;
        m_saveData.ObjectActive = ObjectActive;
        m_saveData.AnimationActive = AnimationActive;

        await UniTask.Delay(100);

        InteractableSaveData LoadedData = null;
        while ((LoadedData = m_saveChannel.RaiseObjectSaveDataRequest(name)) == null) await UniTask.Delay(100);
        if (LoadedData.New) return;

        Debug.Log($"{name}, has old object data: Loading.....");
        m_saveData = LoadedData;
        OnDataLoaded?.Invoke(LoadedData);
    }

    #region General Interactable object Override
    public override void UpdateCanInterect(bool CanInterect)
    {
        m_saveData.CanInteract = CanInterect;
        m_saveChannel.RaiseObjectSaveDataUpdated(m_saveData);
    }
    public override void UpdateCanPrint(bool CanPrint)
    {
        m_saveData.CanPrint = CanPrint;
        m_saveChannel.RaiseObjectSaveDataUpdated(m_saveData);
    }
    public override void UpdateCanScan(bool CanScan)
    {
        m_saveData.CanScan = CanScan;
        m_saveChannel.RaiseObjectSaveDataUpdated(m_saveData);
    }
    public override void UpdateObjectActivation(bool Active)
    {
        m_saveData.ObjectActive = Active;
        m_saveChannel.RaiseObjectSaveDataUpdated(m_saveData);
    }
    public override void UpdateAnimationActivation(bool Active)
    {
        m_saveData.AnimationActive = Active;
        m_saveChannel.RaiseObjectSaveDataUpdated(m_saveData);
    }
    #endregion
}