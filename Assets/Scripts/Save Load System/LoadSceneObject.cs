using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class LoadSceneObject : MonoBehaviour
{
    public static LoadSceneObject Instance;

    [SerializeField] private SceneType thisScene = SceneType.None;

    public bool AllLoad { get; private set; } = false;
    public SceneType CurrentScene => thisScene;

    async private void Awake()
    {
        Instance = this;

        await UniTask.WaitUntil(() => SaveSystem.Instance.SaveData != null);

        if (thisScene == SceneType.TutorialScene) LoadTutorialSceneObject();

        AllLoad = true;
    }

    #region Tutorial Scene
    [SerializeField] private TutorialSceneObject tutorialSceneObject = null;

    private void LoadTutorialSceneObject()
    {
        SaveData loadedSaveData = SaveSystem.Instance.SaveData;

        tutorialSceneObject.awakeDialog.SetActive(loadedSaveData.TutorialScene.JustAwake);
        tutorialSceneObject.tablet.SetActive(!loadedSaveData.TutorialScene.TakeTablet);
        tutorialSceneObject.interactionQuide.SetActive(!loadedSaveData.TutorialScene.InteractionQuideInteracted);
    }
    #endregion
}

public enum SceneType
{
    None,
    TutorialScene,
    SelectionScene
}

[Serializable] public class TutorialSceneObject
{
    public GameObject tablet = null;
    public GameObject awakeDialog = null;
    public GameObject interactionQuide = null;
}