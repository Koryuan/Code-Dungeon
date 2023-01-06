using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    #region Instance
    private static SaveLoadSystem m_instance;
    public static SaveLoadSystem Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("Save System").AddComponent<SaveLoadSystem>();
                DontDestroyOnLoad(m_instance.gameObject);
            }
            return m_instance;
        }
    }
    #endregion

    #region Save Data
    private void Awake() => _SaveData = new SaveData();
    public SaveData _SaveData { get; private set; } = null;
    #endregion

    public void NewSaveData() => _SaveData = new SaveData();

    #region Save and Load
    public bool LoadFromSaveData = false;

    public void SaveFile(string FileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + FileName + ".json");
        var json = JsonUtility.ToJson(_SaveData, false);

        Debug.Log($"Awake: {_SaveData.TutorialScene.JustAwake} at Save");

        bf.Serialize(file, json);
        file.Close();
    }
    private SaveData LoadFileForDebug()
    {
        if (!DebugSaveFileExist || DebuggingTool.Instance == null) return new SaveData();

        // Create initializer to get data
        SaveData loadData = new SaveData();
        BinaryFormatter bf = new BinaryFormatter();

        // Get Data from json file
        FileStream file = File.Open(Application.persistentDataPath + "/Save Data.json", FileMode.Open);
        JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), loadData);
        file.Close();
        return loadData;
    }
    public void LoadData(string SaveName)
    {
        _SaveData = LoadFile(SaveName);
        LoadFromSaveData = true;
        Debug.Log($"Awake: {_SaveData.TutorialScene.JustAwake} at load game");

        
        SceneLoad.LoadStageFromSaveFile();
    }
    public SaveData LoadFile(string FileName)
    {
        if (!FileExist(FileName)) return null;

        // Create initializer to get data
        SaveData loadData = new SaveData();
        BinaryFormatter bf = new BinaryFormatter();

        // Get Data from json file
        FileStream file = File.Open(Application.persistentDataPath + "/" + FileName + ".json", FileMode.Open);
        JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), loadData);
        file.Close();
        return loadData;
    }
    #endregion

    #region Check Exist
    public bool SaveFileExist
    {
        get
        {
            if (File.Exists(Application.persistentDataPath + "/Save Data - 1.json")) return true;
            if (File.Exists(Application.persistentDataPath + "/Save Data - 2.json")) return true;
            if (File.Exists(Application.persistentDataPath + "/Save Data - 3.json")) return true;
            return false;
        }
    }
    public bool DebugSaveFileExist
    {
        get
        {
            if (File.Exists(Application.persistentDataPath + "/Save Data.json")) return true;
            return false;
        }
    }
    private bool FileExist(string FileName)
    {
        if (File.Exists(Application.persistentDataPath + "/" + FileName + ".json")) return true;
        return false;
    }
    #endregion
}