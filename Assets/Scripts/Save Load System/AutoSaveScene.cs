using System.Collections;
using UnityEngine;

public static class AutoSaveScene
{
    public static void SaveObjectState(string ObjectName)
    {
        if (LoadSceneObject.Instance.CurrentScene == SceneType.TutorialScene)
            SaveTutorialScene(ObjectName);
    }

    private static void SaveTutorialScene(string ObjectName)
    {
        SaveDataTutorialScene tutorialSaveData = SaveSystem.Instance.CurrentSaveData.TutorialScene;
        if (tutorialSaveData == null)
        {
            Debug.LogError("The Save Data have no Tutorial Class attached");
            return;
        }
            
        switch (ObjectName)
        {
            case "Awake - Dialog":
                tutorialSaveData.JustAwake = false;
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
}