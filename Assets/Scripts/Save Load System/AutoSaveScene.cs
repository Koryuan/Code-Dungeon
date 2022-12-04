using UnityEngine;

public static class AutoSaveScene
{
    public static void SaveObjectState(string ObjectName)
    {
        if (SaveLoadSystem.Instance == null || SaveLoadSystem.Instance._SaveData == null) return;
        if (LoadSceneObject.Instance.CurrentScene == SceneType.TutorialScene)
            SaveTutorialScene(ObjectName);
        else if (LoadSceneObject.Instance.CurrentScene == SceneType.Print1Scene)
            SavePrint1Scene(ObjectName);
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

    private static void SavePrint1Scene(string ObjectName)
    {
        var print1SaveData = SaveLoadSystem.Instance._SaveData.Print1Scene;
        if (print1SaveData == null)
        {
            Debug.LogError("The Save Data have no Tutorial Class attached");
            return;
        }

        switch (ObjectName)
        {
            case "Code Machine - 1":
                print1SaveData.UpdateCodeMachine1Test = true;
                break;
            case "Door - 1":
                print1SaveData.OpenDoor1 = true;
                break;
            case "Item Print 1":
                print1SaveData.TakePrint1Item = true;
                break;
            default:
                break;
        }
    }
}