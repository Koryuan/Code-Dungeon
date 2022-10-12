using UnityEngine.SceneManagement;

public static class SceneLoad
{
    public static void LoadMainMenu() => SceneManager.LoadScene("Main Menu Scene");
    public static void LoadTutorialMap() => SceneManager.LoadScene("Tutorial Scene");
    public static void LoadSelectStage() => SceneManager.LoadScene("Stage Selection");
}