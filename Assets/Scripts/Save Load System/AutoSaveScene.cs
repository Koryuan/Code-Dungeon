﻿using System;
using UnityEngine;

public static class AutoSaveScene
{
    public static void SaveObjectState(string ObjectName, string Additional = "")
    {
        if (SaveLoadSystem.Instance == null || SaveLoadSystem.Instance._SaveData == null) return;
        if (LoadSceneObject.Instance.CurrentScene == SceneType.TutorialScene)
            SaveTutorialScene(ObjectName);
        else if (LoadSceneObject.Instance.CurrentScene == SceneType.Print1Scene)
            SavePrint1Scene(ObjectName, Additional);
        else if (LoadSceneObject.Instance.CurrentScene == SceneType.Print2Scene)
            SavePrint2Scene(ObjectName, Additional);
    }
    public static void SaveObjectState<T>(string ObjectName,T Additional)
    {
        if (SaveLoadSystem.Instance == null || SaveLoadSystem.Instance._SaveData == null) return;
        if (LoadSceneObject.Instance.CurrentScene == SceneType.Print2Scene)
            SavePrint2Scene(ObjectName, Additional);
    }

    private static void SaveTutorialScene(string ObjectName)
    {
        var tutorialSaveData = SaveLoadSystem.Instance._SaveData.TutorialScene;
        if (tutorialSaveData == null)
        {
            Debug.LogError("The Save Data have no Tutorial Class attached");
            return;
        }
            
        switch (ObjectName)
        {
            case "Awake - Dialog":
                tutorialSaveData.JustAwake = false;
                Debug.Log($"Just Awake: {SaveLoadSystem.Instance._SaveData.TutorialScene.JustAwake}");
                break;
            case "Tablet":
                tutorialSaveData.TakeTablet = true;
                break;
            case "Player Trigger - How to play":
                tutorialSaveData.InteractionQuideInteracted = true;
                break;
            default:
                break;
        }
    }

    private static void SavePrint1Scene(string ObjectName, string Additional = "")
    {
        var print1SaveData = SaveLoadSystem.Instance._SaveData.Print1Scene;
        if (print1SaveData == null)
        {
            Debug.LogError("The Save Data have no Class attached");
            return;
        }

        switch (ObjectName)
        {
            // Puzzle 1
            case "Code Machine - 1":
                print1SaveData.UpdateCodeMachine1Test = true;
                break;
            case "Code Machine - 1 | Print":
                print1SaveData.CodeMachine1PrintedText = "Hello World";
                break;
            case "Door - 1":
                print1SaveData.OpenDoor1 = true;
                break;
            case "Item Print 1":
                print1SaveData.TakePrint1Item = true;
                break;

            // Puzzle 2
            case "Treasure Chest - 2":
                print1SaveData.TreasureChest2_Taken = true;
                break;
            case "Treasure Chest - 2 | Open":
                print1SaveData.TreasureChest2_Opened = true;
                break;
            case "Door - 2":
                print1SaveData.OpenDoor2 = true;
                break;
            case "Code Machine - 2":
                print1SaveData.CodeMachine2Updated = true;
                break;
            case "Code Machine - 2 | Print":
                print1SaveData.CodeMachine2PrintedText = "729";
                break;
            case "Keypad":
                print1SaveData.Keypad_LastText = Additional;
                break;
            default:
                break;
        }
    }

    private static void SavePrint2Scene<T>(string ObjectName, T Additional)
    {
        var SaveData = SaveLoadSystem.Instance._SaveData.Print2Scene;
        if (SaveData == null)
        {
            Debug.LogError("The Save Data have no Class attached");
            return;
        }

        switch (ObjectName)
        {
            // Puzzle 1
            case "Door 1":
                Debug.Log("Door 1 Open: Saved");
                SaveData.Door1OpenOnce = true;
                SaveData.Door1Open = true;
                break;
            case "Door 1 | Close":
                Debug.Log("Door 1 Close: Saved");
                SaveData.Door1Open = false;
                break;
            case "NPC - 1":
                if (Additional is int)
                {
                    Debug.Log("Save NPC");
                    SaveData.NPCDialog = Convert.ToInt32(Additional);
                }
                break;
            case "Code Machine - 1":

                break;
            default:
                break;
        }
    }
}