using Cysharp.Threading.Tasks;

public class AutoSaveTriggerEnter : AutoSaveAttach
{
    private TriggerEnterSaveData m_saveData = new TriggerEnterSaveData();

    async public void LoadData()
    {
        m_saveData.ID = name;

        await UniTask.Delay(100);

        TriggerEnterSaveData LoadedData = null;
        while ((LoadedData = m_saveChannel.RaiseTriggerEnterSaveDataRequest(name)) == null) await UniTask.Delay(100);
        if (LoadedData.New) return;

        m_saveData = LoadedData;
        OnDataLoaded?.Invoke(LoadedData);
    }

    public void UpdateObjectActivation(bool Active)
    {
        m_saveData.ObjectActivation = Active;
        m_saveChannel.RaiseTriggerEnterSaveDataUpdated(m_saveData);
    }
}

[System.Serializable] public class TriggerEnterSaveData : SaveDataAuto
{
    public bool ObjectActivation = true;
}