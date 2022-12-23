using System;
using UnityEngine;

public static class AutoSaveScene
{
    public static void SaveObjectState(string ObjectName, string Additional = "")
    {
        if (SaveLoadSystem.Instance == null || SaveLoadSystem.Instance._SaveData == null) return;
        if (LoadSceneObject.Instance.CurrentScene == SceneType.TutorialScene)
            SaveTutorialScene(ObjectName);
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
            #region Puzzle 4
            case "Code Machine - Integer - 01 | Print":
                Debug.Log("Save: Code Machine - Integer - 01 | Print");
                SaveData.Puzzle4_AddToCollective("Code Machine - Integer - 01");
                break;
            case "Code Machine - Integer - 01 | Close":
                Debug.Log("Save: Code Machine - Integer - 01 | Close");
                SaveData.Puzzle4_CodeMachineIntInterected = true;
                break;
            case "Code Machine - Char - 01 | Print":
                Debug.Log("Save: Code Machine - Char - 01 | Print");
                SaveData.Puzzle4_AddToCollective("Code Machine - Char - 01");
                break;
            case "Code Machine - Char - 01 | Close":
                Debug.Log("Save: Code Machine - Char - 01 | Close");
                SaveData.Puzzle4_CodeMachineCharInterected = true;
                break;
            case "Code Machine - Float - 01 | Print":
                Debug.Log("Save: Code Machine - Float - 01 | Print");
                SaveData.Puzzle4_AddToCollective("Code Machine - Float - 01");
                break;
            case "Code Machine - Float - 01 | Close":
                Debug.Log("Save: Code Machine - Float - 01 | Close");
                SaveData.Puzzle4_CodeMachineFloatInterected = true;
                break;
            case "Code Machine - String - 01 | Print":
                Debug.Log("Save: Code Machine - String - 01 | Print");
                SaveData.Puzzle4_AddToCollective("Code Machine - String - 01");
                break;
            case "Code Machine - String - 01 | Close":
                Debug.Log("Save: Code Machine - String - 01 | Close");
                SaveData.Puzzle4_CodeMachineStringInterected = true;
                break;
            case "Puzzle 4 - Door":
                Debug.Log("Save: Puzzle 4 - Door | Open");
                SaveData.Puzzle4_DoorOpen = true;
                break;
            #endregion
            default:
                break;
        }
    }
}