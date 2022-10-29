using System;
using UnityEngine.SceneManagement;

public static class SceneLoad
{
    public static void LoadMainMenu() => SceneManager.LoadScene("Main Menu Scene");
    public static void LoadTutorialMap() => SceneManager.LoadScene("Tutorial Scene");
    public static void LoadSelectStage() => SceneManager.LoadScene("Stage Selection");
    public static void LoadStageFromSaveFile()
    {
        var saveData = SaveLoadSystem.Instance._SaveData;
        if (DebuggingTool.Instance == null)
        {
            switch(saveData.LastSceneName)
            {
                case SceneType.TutorialScene:
                    LoadTutorialMap();
                    break;
                case SceneType.SelectionScene:
                    LoadSelectStage();
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (saveData.LastSceneName)
            {
                case SceneType.TutorialScene:
                    SceneManager.LoadScene("(Duplicate) Tutorial Scene");
                    break;
                default:
                    break;
            }
        }
    }
}