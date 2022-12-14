using Cysharp.Threading.Tasks;
using UnityEngine;

public class AutoSaveDoor : AutoSaveAttach
{
    private DoorSaveData m_saveData = new DoorSaveData();

    async public void LoadData(bool Open)
    {
        m_saveData.ID = name;
        m_saveData.Open = Open;

        await UniTask.Delay(100);

        DoorSaveData LoadedData = null;
        while ((LoadedData = m_saveChannel.RaiseDoorSaveDataRequest(name)) == null) await UniTask.Delay(100);
        if (LoadedData.New) return;

        Debug.Log($"{name}, has old door data: Loading.....");
        m_saveData = LoadedData;
        OnDataLoaded?.Invoke(LoadedData);
    }

    public void UpdateOpen(bool Open)
    {
        m_saveData.Open = Open;
        m_saveChannel.RaiseDoorSaveDataUpdated(m_saveData);
        Debug.Log($"{name}, Save Door Open data");
    }
}

[System.Serializable] public class DoorSaveData : SaveDataAuto
{
    public bool Open;
}