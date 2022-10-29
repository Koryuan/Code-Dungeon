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

        await UniTask.WaitUntil(() => SaveLoadSystem.Instance._SaveData != null);
        SaveLoadSystem.Instance._SaveData.LastSceneName = thisScene;

        if (thisScene == SceneType.TutorialScene) LoadTutorialSceneObject();

        PlayerController player = FindObjectOfType<PlayerController>();

        if (player != null && SaveLoadSystem.Instance.LoadFromSaveData)
        {
            player.gameObject.transform.position = SaveLoadSystem.Instance._SaveData.PlayerLastPosition;
            SaveLoadSystem.Instance.LoadFromSaveData = false;
        }

        AllLoad = true;
    }

    #region Tutorial Scene
    [SerializeField] private TutorialSceneObject tutorialSceneObject = null;

    private void LoadTutorialSceneObject()
    {
        void CheckNullReferences()
        {
            if (!tutorialSceneObject.awakeDialog) Debug.LogError($"{name} has no Awake Dialog reference");
            if (!tutorialSceneObject.tablet) Debug.LogError($"{name} has no Tablet reference");
            if (!tutorialSceneObject.interactionQuide) Debug.LogError($"{name} has no Guide reference");
        }

        SaveData loadedSaveData = SaveLoadSystem.Instance._SaveData;

        CheckNullReferences();

        Debug.Log($"Awake: {loadedSaveData.TutorialScene.JustAwake} at load scene");

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