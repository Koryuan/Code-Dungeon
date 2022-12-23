using System;
using UnityEngine.SceneManagement;

public static class SceneLoad
{
    public static void LoadMainMenu() => SceneManager.LoadScene("Main Menu Scene");
    public static void LoadTutorialMap() => SceneManager.LoadScene("Tutorial Scene");
    public static void LoadSelectStage() => SceneManager.LoadScene("Stage Selection");
    public static void LoadPrint1Stage() => SceneManager.LoadScene("Print 1");
    public static void LoadPrint2Stage() => SceneManager.LoadScene("Print 2");
    public static void LoadSelectionStage() => SceneManager.LoadScene("Selection");
    public static void LoadStageFromSaveFile()
    {
        var saveData = SaveLoadSystem.Instance._SaveData;
        switch(saveData.LastScene)
        {
            case SceneType.TutorialScene:
                LoadTutorialMap();
                break;
            case SceneType.SelectStageScene:
                LoadSelectStage();
                break;
            case SceneType.Print1Scene:
                LoadPrint1Stage();
                break;
            case SceneType.Print2Scene:
                LoadPrint2Stage();
                break;
            case SceneType.SelectionScene:
                LoadSelectionStage();
                break;
            default:
                break;
        }
    }
}