
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AutoSaveTreasureChest : AutoSaveInterectable
{
    private TreasureChestSaveData m_saveData = new TreasureChestSaveData();

    public void AdditionalData(bool Open, int OnOpenIndex, int OnCloseIndex)
    {
        m_saveData.Open = Open;
        m_saveData.OnOpenIndex = OnOpenIndex;
        m_saveData.OnCloseIndex = OnCloseIndex;
    }

    async public override void LoadData(bool CanInterect, bool CanPrint, bool CanScan, bool ObjectActive, bool AnimationActive)
    {
        m_saveData.ID = name;
        m_saveData.CanInteract = CanInterect;
        m_saveData.CanPrint = CanPrint;
        m_saveData.CanScan = CanScan;
        m_saveData.ObjectActive = ObjectActive;
        m_saveData.AnimationActive = AnimationActive;

        await UniTask.Delay(100);

        TreasureChestSaveData LoadedData = null;
        while ((LoadedData = m_saveChannel.RaiseTreasureChestSaveDataRequest(name)) == null) await UniTask.Delay(100);
        if (LoadedData.New)
        {
            Debug.Log($"{name}, didn't need load data");
            return;
        }

        Debug.Log($"{name}, has old treasure chest data: Loading.....");
        m_saveData = LoadedData;
        OnDataLoaded?.Invoke(LoadedData);
    }

    public void UpdateOpen(bool Open)
    {
        m_saveData.Open = Open;
        m_saveChannel.RaiseTreasureChestSaveDataUpdated(m_saveData);
    }
    public void UpdateOpenIndex(int Index)
    {
        m_saveData.OnOpenIndex = Index;
        m_saveChannel.RaiseTreasureChestSaveDataUpdated(m_saveData);
    }
    public void UpdateCloseIndex(int Index)
    {
        m_saveData.OnCloseIndex = Index;
        m_saveChannel.RaiseTreasureChestSaveDataUpdated(m_saveData);
    }

    #region General Interactable object Override
    public override void UpdateCanInterect(bool CanInterect)
    {
        m_saveData.CanInteract = CanInterect;
        m_saveChannel.RaiseTreasureChestSaveDataUpdated(m_saveData);
    }
    public override void UpdateCanPrint(bool CanPrint)
    {
        m_saveData.CanPrint = CanPrint;
        m_saveChannel.RaiseTreasureChestSaveDataUpdated(m_saveData);
    }
    public override void UpdateCanScan(bool CanScan)
    {
        m_saveData.CanScan = CanScan;
        m_saveChannel.RaiseTreasureChestSaveDataUpdated(m_saveData);
    }
    public override void UpdateObjectActivation(bool Active)
    {
        m_saveData.ObjectActive = Active;
        m_saveChannel.RaiseTreasureChestSaveDataUpdated(m_saveData);
    }
    public override void UpdateAnimationActivation(bool Active)
    {
        m_saveData.AnimationActive = Active;
        m_saveChannel.RaiseTreasureChestSaveDataUpdated(m_saveData);
    }
    #endregion
}

[System.Serializable] public class TreasureChestSaveData : InteractableSaveData
{
    public bool Open;
    public int OnOpenIndex = -1;
    public int OnCloseIndex = -1;
}