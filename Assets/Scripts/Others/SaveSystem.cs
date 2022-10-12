using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem m_instance;
    public static SaveSystem Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("Save System").AddComponent<SaveSystem>();
                DontDestroyOnLoad(m_instance.gameObject);
            }
            return m_instance;
        }
    }

    private void Awake() => SaveData = CurrentSaveData = LoadFile();

    #region Save Data
    public SaveData SaveData { get; private set; } = null;
    public SaveData CurrentSaveData { get; private set; } = null;

    #region Item List
    public List<Item> InventoryItemList
    {
        get
        {
            List<Item> newList = new List<Item>();
            newList.AddRange(SaveData._itemList);
            return newList;
        }
    }
    public void UpdateItemList(List<Item> NewItemList) => SaveData._itemList = NewItemList;
    #endregion
    #endregion

    #region Save, Load, and Check
    private void SaveFile(SaveData DataToSave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Save Data.json");
        var json = JsonUtility.ToJson(DataToSave, false);

        bf.Serialize(file, json);
        file.Close();
    }
    private SaveData LoadFile()
    {
        if (!SaveFileExist || DebuggingTool.Instance == null) return new SaveData();

        // Create initializer to get data
        SaveData loadData = new SaveData();
        BinaryFormatter bf = new BinaryFormatter();

        // Get Data from json file
        FileStream file = File.Open(Application.persistentDataPath + "/Save Data.json", FileMode.Open);
        JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), loadData);
        file.Close();
        return loadData;
    }
    public bool SaveFileExist
    {
        get
        {
            if (!File.Exists(Application.persistentDataPath + "/Save Data.json")) return false;
            return true;
        }
    }
    #endregion
}