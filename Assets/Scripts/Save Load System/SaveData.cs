using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    // Showned Data in Save/Load Menu
    public SceneType LastScene = SceneType.None;
    public List<Item> _ItemList = new List<Item>();

    // Player Data
    public Vector3 PlayerLastPosition = new Vector3();

    // Scene Data
    public SaveDataTutorialScene TutorialScene = new SaveDataTutorialScene();
    public SaveDataPrint1Scene Print1Scene = new SaveDataPrint1Scene();

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
    public bool TakePrint1Item = false;
    public bool UpdateCodeMachine1Test = false;
    public bool OpenDoor1 = false;

    public void ResetData()
    {
        TakePrint1Item = false;
        UpdateCodeMachine1Test = false;
        OpenDoor1 = false;
    }
}