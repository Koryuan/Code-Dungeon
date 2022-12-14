using Cysharp.Threading.Tasks;
using UnityEngine;

public class AutoSaveNPC : AutoSaveInterectable
{
    private NPCSaveData m_saveData = new NPCSaveData();

    async public override void LoadData(bool CanInterect, bool CanPrint, bool CanScan, bool ObjectActive, bool AnimationActive)
    {
        m_saveData.ID = name;
        m_saveData.CanInteract = CanInterect;
        m_saveData.CanPrint = CanPrint;
        m_saveData.CanScan = CanScan;
        m_saveData.ObjectActive = ObjectActive;
        m_saveData.AnimationActive = AnimationActive;

        await UniTask.Delay(100);

        NPCSaveData LoadedData = null;
        while ((LoadedData = m_saveChannel.RaiseNPCSaveDataRequest(name)) == null) await UniTask.Delay(100);
        if (LoadedData.New) return;

        Debug.Log($"{name}, has old npc data: Loading.....");
        m_saveData = LoadedData;
        OnDataLoaded?.Invoke(LoadedData);
    }

    public void UpdateCurrentIndex(int Index)
    {
        m_saveData.CurrentIndex = Index;
        m_saveChannel.RaiseNPCSaveDataUpdated(m_saveData);
    }

    #region General Interactable object Override
    public override void UpdateCanInterect(bool CanInterect)
    {
        m_saveData.CanInteract = CanInterect;
        m_saveChannel.RaiseNPCSaveDataUpdated(m_saveData);
    }
    public override void UpdateCanPrint(bool CanPrint)
    {
        m_saveData.CanPrint = CanPrint;
        m_saveChannel.RaiseNPCSaveDataUpdated(m_saveData);
    }
    public override void UpdateCanScan(bool CanScan)
    {
        m_saveData.CanScan = CanScan;
        m_saveChannel.RaiseNPCSaveDataUpdated(m_saveData);
    }
    public override void UpdateObjectActivation(bool Active)
    {
        m_saveData.ObjectActive = Active;
        m_saveChannel.RaiseNPCSaveDataUpdated(m_saveData);
    }
    public override void UpdateAnimationActivation(bool Active)
    {
        m_saveData.AnimationActive = Active;
        m_saveChannel.RaiseNPCSaveDataUpdated(m_saveData);
    }
    #endregion
}

[System.Serializable] public class NPCSaveData : InteractableSaveData
{
    public int CurrentIndex = 0;
}