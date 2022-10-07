using UnityEngine.SceneManagement;

public static class SceneLoadManager
{
    public static void LoadTutorialMap() => SceneManager.LoadScene("Tutorial Scene");
    public static void LoadSelectStage() => SceneManager.LoadScene("Stage Selection");
}