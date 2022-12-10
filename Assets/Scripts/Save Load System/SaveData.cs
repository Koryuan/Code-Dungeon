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