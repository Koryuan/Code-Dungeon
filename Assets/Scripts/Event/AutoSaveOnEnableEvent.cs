using Cysharp.Threading.Tasks;

public class AutoSaveOnEnableEvent : AutoSaveAttach
{
    private OnEnableEventSaveData m_saveData = new OnEnableEventSaveData();

    async public void LoadData(bool Enabled)
    {
        m_saveData.ID = name;
        m_saveData.Enabled = Enabled;
        //m_saveData.CurrentIndex = CurrentIndex;

        await UniTask.Delay(100);

        OnEnableEventSaveData LoadedData = null;
        while ((LoadedData = m_saveChannel.RaiseOnEnableEventSaveDataRequest(name)) == null) await UniTask.Delay(100);
        if (LoadedData.New)
        {
            OnDataLoaded?.Invoke(null);
            return;
        }

        m_saveData = LoadedData;
        OnDataLoaded?.Invoke(LoadedData);
    }

    public void UpdateEnable(bool Enabled)
    {
        m_saveData.Enabled = Enabled;
        m_saveChannel.RaiseOnEnableEventSaveDataUpdated(m_saveData);
    }
}

[System.Serializable] public class OnEnableEventSaveData : SaveDataAuto
{
    public bool Enabled = false;
    //public int CurrentIndex = -1;
}
