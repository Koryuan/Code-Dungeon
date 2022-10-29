using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    // Showned Data in Save/Load Menu
    public SceneType LastSceneName = SceneType.None;
    public List<Item> _ItemList = new List<Item>();

    // Player Data
    public Vector3 PlayerLastPosition = new Vector3();

    public SaveDataTutorialScene TutorialScene = new SaveDataTutorialScene();

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
    public bool UseNextSceneDoor = false;
}