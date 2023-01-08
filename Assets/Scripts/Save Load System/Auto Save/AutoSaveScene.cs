using UnityEditor;
using UnityEngine;

public static class AutoSaveScene
{
    public static void SaveObjectState(string ObjectName, string Additional = "")
    {
        if (SaveLoadSystem.Instance == null || SaveLoadSystem.Instance._SaveData == null) return;
        if (LoadSceneObject.Instance.CurrentScene == SceneType.TutorialScene)
            SaveTutorialScene(ObjectName);
        else if (LoadSceneObject.Instance.CurrentScene == SceneType.SelectionScene)
            SaveSelectionScene(ObjectName);
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
                //Debug.Log($"Just Awake: {SaveLoadSystem.Instance._SaveData.TutorialScene.JustAwake}");
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
    private static void SaveSelectionScene(string ObjectName)
    {
        if (SaveLoadSystem.Instance._SaveData == null)
        {
            Debug.LogError("There is no save data");
            return;
        }

        if (ObjectName == "Puzzle 16 - Door - Stage Select - 1")
            SaveLoadSystem.Instance._SaveData.OtherData.Stage2DoorOpen = true;
    }
}