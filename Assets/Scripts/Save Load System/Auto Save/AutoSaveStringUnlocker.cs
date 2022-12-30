using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveStringUnlocker : AutoSaveAttach
{
    private StringUnlockerSaveData m_saveData = new StringUnlockerSaveData();

    async public void LoadData()
    {
        m_saveData.ID = name;

        await UniTask.Delay(100);

        StringUnlockerSaveData LoadedData = null;
        while ((LoadedData = m_saveChannel.RaiseStringUnlockerSaveDataRequest(name)) == null) await UniTask.Delay(100);
        if (LoadedData.New) return;

        Debug.Log($"{name}, has old door data: Loading.....");
        m_saveData = LoadedData;
        OnDataLoaded?.Invoke(LoadedData);
    }

    public void UpdateStringList(string AddedString)
    {
        m_saveData.UnlockedString.Add(AddedString);
        m_saveChannel.RaiseStringUnlockerSaveDataUpdated(m_saveData);
        //Debug.Log($"{name}, Save Door Open data");
    }
}

[System.Serializable] public class StringUnlockerSaveData : SaveDataAuto
{
    public List<string> UnlockedString = new List<string>();
}