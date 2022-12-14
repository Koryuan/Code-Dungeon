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
    public SaveDataPrint1Scene Print1Scene = new SaveDataPrint1Scene();
    public SaveDataPrint2Scene Print2Scene = new SaveDataPrint2Scene();

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
            Debug.Log($"Save Data:Door ({NewData.ID}) | update Data");
        }
        else
        {
            DoorSaveDataList.Add(NewData);
            Debug.Log($"Save Data:Door ({NewData.ID}) | add Data");
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
            Debug.Log($"Save Data:Code Machine ({NewData.ID}) | update Data");
        }
        else
        {
            CodeMachineSaveDataList.Add(NewData);
            Debug.Log($"Save Data:Code Machine ({NewData.ID}) | add Data");
        }
    }
    #endregion

    // Save Data Description
    public string SaveFileName = "";
    public float PlayTime = 0f;
    public string LastChapterName = "";
    public string PlayTimeText => $"{PlayTime / 3600f:000}:{Math.Truncate((PlayTime % 3600) / 60):00}:{PlayTime % 60:00}";
}

[Serializable] public class SaveDataTutorialScene
{
    public bool JustAwake = true;
    public bool InteractionQuideInteracted = false;
    public bool TakeTablet = false;

    public void ResetData() => 
        (JustAwake, InteractionQuideInteracted, TakeTablet) = (true,false,false);
}

[Serializable] public class SaveDataPrint1Scene
{
    // Puzzle 1
    public bool TakePrint1Item = false;
    public bool UpdateCodeMachine1Test = false;
    public bool OpenDoor1 = false;
    public string CodeMachine1PrintedText = string.Empty;

    // Puzzle 2
    public bool CodeMachine2Updated = false;
    public string CodeMachine2PrintedText = string.Empty;
    public bool OpenDoor2 = false;
    public bool TreasureChest2_Opened = false;
    public bool TreasureChest2_Taken = false;
    public string Keypad_LastText = string.Empty;

    public void ResetData()
    {
        //Puzzle 1
        TakePrint1Item = false;
        UpdateCodeMachine1Test = false;
        OpenDoor1 = false;
        CodeMachine1PrintedText = string.Empty;

        // Puzzle 2
        CodeMachine2Updated = false;
        CodeMachine2PrintedText = string.Empty;
        OpenDoor2 = false;
        TreasureChest2_Opened = false;
        TreasureChest2_Taken = false;
        Keypad_LastText = string.Empty;
    }
}

[Serializable] public class SaveDataPrint2Scene
{
    //Puzzle 3
    public bool Door1Open = false;
    public bool Door1OpenOnce = false;
    public int NPCDialog = -1;
    public InputFieldLine CodeMachine1InputField = new InputFieldLine();
    public string CodeMachine1PrintedText = string.Empty;

    // Puzzle 4
    public bool Puzzle4_CodeMachineIntInterected = false;
    public bool Puzzle4_CodeMachineCharInterected = false;
    public bool Puzzle4_CodeMachineStringInterected = false;
    public bool Puzzle4_CodeMachineFloatInterected = false;
    public bool Puzzle4_DoorOpen = false;

    public List<string> Puzzle4_Collective = new List<string>();
    public void Puzzle4_AddToCollective(string Text)
    {
        foreach (string text in Puzzle4_Collective)
            if (Text == text) return;
        Puzzle4_Collective.Add(Text);
    }
}