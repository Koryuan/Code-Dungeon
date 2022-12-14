using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveKeypad : AutoSaveAttach
{
    private KeypadSaveData m_saveData = new KeypadSaveData();

    async public void LoadData()
    {
        m_saveData.ID = name;

        await UniTask.Delay(100);

        KeypadSaveData LoadedData = null;
        while ((LoadedData = m_saveChannel.RaiseKeypadSaveDataRequest(name)) == null) await UniTask.Delay(100);
        if (LoadedData.New) return;

        Debug.Log($"{name}, has old keypad data: Loading.....");
        m_saveData = LoadedData;
        OnDataLoaded?.Invoke(LoadedData);
    }

    public void UpdateOccuredText(string OccuredText)
    {
        m_saveData.OccuredText.Add(OccuredText);
        m_saveChannel.RaiseKeypadSaveDataUpdated(m_saveData);
    }
    public void UpdateCurrentText(string CurrentText)
    {
        m_saveData.LastText = CurrentText;
        m_saveChannel.RaiseKeypadSaveDataUpdated(m_saveData);
    }
}

[System.Serializable] public class KeypadSaveData : SaveDataAuto
{
    public List<string> OccuredText = new List<string>();
    public string LastText = "";
}