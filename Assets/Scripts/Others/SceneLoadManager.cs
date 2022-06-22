using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoadManager
{
    public static void LoadTutorialMap() => SceneManager.LoadScene("Tutorial Scene");
}