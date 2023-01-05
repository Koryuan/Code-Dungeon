using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    // Showned Data in Save/Load Menu
    public SceneType LastScene = SceneType.None;
    public List<Item> _ItemList = new List<Item>();
    public List<HelpSettings> HelpList = new List<HelpSettings>();

    // Player Data
    public Vector3 PlayerLastPosition = new Vector3();

    // Option Data
    public float sfxVolume = 1f;
    public float bgmVolume = 1f;

    // Scene Data
    public SaveDataTutorialScene TutorialScene = new SaveDataTutorialScene();
    public OtherSceneData OtherData = new OtherSceneData();

    #region Scene Object Save Data
    public List<InteractableSaveData> ObjectSaveDataList = new List<InteractableSaveData>();
    public InteractableSaveData GetObjectSaveData(string ID) => ObjectSaveDataList.Find(x => x.ID == ID);
    public void AddObjectSaveData(InteractableSaveData NewData)
    {
        InteractableSaveData OldData = ObjectSaveDataList.Find(x => x == NewData);
        NewData.New = false;

        if (OldData != null) OldData = NewData;
        else ObjectSaveDataList.Add(NewData);

        Debug.Log("Save Data: Object Add/Update Data");
    }
    #endregion

    #region Scene NPC Save Data
    public List<NPCSaveData> NPCSaveDataList = new List<NPCSaveData>();
    public NPCSaveData GetNPCSaveData(string ID) => NPCSaveDataList.Find(x => x.ID == ID);
    public void AddNPCSaveData(NPCSaveData NewData)
    {
        NPCSaveData OldData = NPCSaveDataList.Find(x => x == NewData);
        NewData.New = false;

        if (OldData != null)
        {
            OldData = NewData;
            Debug.Log($"Save Data:NPC ({NewData.ID}) | update Data");
        }
        else
        {
            NPCSaveDataList.Add(NewData);
            Debug.Log($"Save Data:NPC ({NewData.ID}) | add Data");
        }
    }
    #endregion

    #region Scene Treasure Chest Data
    public List<TreasureChestSaveData> TreasureChestSaveDataList = new List<TreasureChestSaveData>();
    public TreasureChestSaveData GetTreasureChestSaveData(string ID) => TreasureChestSaveDataList.Find(x => x.ID == ID);
    public void AddTreasureChestSaveData(TreasureChestSaveData NewData)
    {
        TreasureChestSaveData OldData = TreasureChestSaveDataList.Find(x => x == NewData);
        NewData.New = false;

        if (OldData != null)
        {
            OldData = NewData;
            Debug.Log($"Save Data:Treasure Chest ({NewData.ID}) | update Data");
        }
        else
        {
            TreasureChestSaveDataList.Add(NewData);
            Debug.Log($"Save Data:Treasure Chest ({NewData.ID}) | add Data");
        }
    }
    #endregion

    #region Scene Keypad Data
    public List<KeypadSaveData> KeypadSaveDataList = new List<KeypadSaveData>();
    public KeypadSaveData GetKeypadSaveData(string ID) => KeypadSaveDataList.Find(x => x.ID == ID);
    public void AddKeypadSaveData(KeypadSaveData NewData)
    {
        KeypadSaveData OldData = KeypadSaveDataList.Find(x => x == NewData);
        NewData.New = false;

        if (OldData != null)
        {
            OldData = NewData;
            Debug.Log($"Save Data:Keypad ({NewData.ID}) | update Data");
        }
        else
        {
            KeypadSaveDataList.Add(NewData);
            Debug.Log($"Save Data:Keypad ({NewData.ID}) | add Data");
        }
    }
    #endregion

    #region Scene Door Data
    public List<DoorSaveData> DoorSaveDataList = new List<DoorSaveData>();
    public DoorSaveData GetDoorSaveData(string ID) => DoorSaveDataList.Find(x => x.ID == ID);
    public void AddDoorSaveData(DoorSaveData NewData)
    {
        DoorSaveData OldData = DoorSaveDataList.Find(x => x == NewData);
        NewData.New = false;

        if (OldData != null)
        {
            OldData = NewData;
            //Debug.Log($"Save Data:Door ({NewData.ID}) | update Data");
        }
        else
        {
            DoorSaveDataList.Add(NewData);
            //Debug.Log($"Save Data:Door ({NewData.ID}) | add Data");
        }
    }
    #endregion

    #region Scene Code Machine Data
    public List<CodeMachineSaveData> CodeMachineSaveDataList = new List<CodeMachineSaveData>();
    public CodeMachineSaveData GetCodeMachineSaveData(string ID) => CodeMachineSaveDataList.Find(x => x.ID == ID);
    public void AddCodeMachineSaveData(CodeMachineSaveData NewData)
    {
        CodeMachineSaveData OldData = CodeMachineSaveDataList.Find(x => x == NewData);
        NewData.New = false;

        if (OldData != null)
        {
            OldData = NewData;
            //Debug.Log($"Save Data:Code Machine ({NewData.ID}) | update Data");
        }
        else
        {
            CodeMachineSaveDataList.Add(NewData);
            //Debug.Log($"Save Data:Code Machine ({NewData.ID}) | add Data");
        }
    }
    #endregion

    #region Scene String Unlocker Data
    public List<StringUnlockerSaveData> StringUnlockerSaveDataList = new List<StringUnlockerSaveData>();
    public StringUnlockerSaveData GetStringUnlockerSaveData(string ID) => StringUnlockerSaveDataList.Find(x => x.ID == ID);
    public void AddStringUnlockerSaveData(StringUnlockerSaveData NewData)
    {
        StringUnlockerSaveData OldData = StringUnlockerSaveDataList.Find(x => x == NewData);
        NewData.New = false;

        if (OldData != null)
        {
            OldData = NewData;
            //Debug.Log($"Save Data:Code Machine ({NewData.ID}) | update Data");
        }
        else
        {
            StringUnlockerSaveDataList.Add(NewData);
            //Debug.Log($"Save Data:Code Machine ({NewData.ID}) | add Data");
        }
    }
    #endregion

    #region Scene String Unlocker Data
    public List<TriggerEnterSaveData> TriggerEnterSaveDataList = new List<TriggerEnterSaveData>();
    public TriggerEnterSaveData GetTriggerEnterSaveData(string ID) => TriggerEnterSaveDataList.Find(x => x.ID == ID);
    public void AddTriggerEnterSaveData(TriggerEnterSaveData NewData)
    {
        TriggerEnterSaveData OldData = TriggerEnterSaveDataList.Find(x => x == NewData);
        NewData.New = false;

        if (OldData != null)
        {
            OldData = NewData;
            //Debug.Log($"Save Data:Code Machine ({NewData.ID}) | update Data");
        }
        else
        {
            TriggerEnterSaveDataList.Add(NewData);
            //Debug.Log($"Save Data:Code Machine ({NewData.ID}) | add Data");
        }
    }
    #endregion

    // Save Data Description
    public string SaveFileName = "";
    public float PlayTime = 0f;
    public string LastChapterName = "";
    public string PlayTimeText => $"{PlayTime / 3600f:000}:{Math.Truncate((PlayTime % 3600) / 60):00}:{PlayTime % 60:00}";
}

[Serializable] public class OtherSceneData
{
    public bool Stage2DoorOpen = false;
}

[Serializable] public class SaveDataTutorialScene
{
    public bool JustAwake = true;
    public bool InteractionQuideInteracted = false;
    public bool TakeTablet = false;

    public void ResetData() => 
        (JustAwake, InteractionQuideInteracted, TakeTablet) = (true,false,false);
}