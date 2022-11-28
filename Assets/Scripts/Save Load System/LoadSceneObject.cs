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

        // Update player position if player is loading save data
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null && SaveLoadSystem.Instance.LoadFromSaveData)
        {
            player.InstantMove(SaveLoadSystem.Instance._SaveData.PlayerLastPosition);
            SaveLoadSystem.Instance.LoadFromSaveData = false;
        }

        if (thisScene == SceneType.TutorialScene) LoadTutorialSceneObject(player);
        else if (thisScene == SceneType.SelectionScene) LoadStageSelectionSceneObject(player);

        SaveLoadSystem.Instance._SaveData.LastScene = thisScene;
        AllLoad = true;
    }

    #region Tutorial Scene
    [SerializeField] private TutorialSceneObject tutorialSceneObject = null;

    private void LoadTutorialSceneObject(PlayerController Player)
    {
        void CheckNullReferences()
        {
            if (!tutorialSceneObject.AwakeDialog) Debug.LogError($"{name} has no Awake Dialog reference");
            if (!tutorialSceneObject.Tablet) Debug.LogError($"{name} has no Tablet reference");
            if (!tutorialSceneObject.InteractionQuide) Debug.LogError($"{name} has no Guide reference");
            if (!tutorialSceneObject.StageSelectionSP) Debug.LogError($"{name} has no spawn point from Stage Selection Scene");
        }

        SaveData loadedSaveData = SaveLoadSystem.Instance._SaveData;

        CheckNullReferences();

        tutorialSceneObject.AwakeDialog.SetActive(loadedSaveData.TutorialScene.JustAwake);
        tutorialSceneObject.Tablet.SetActive(!loadedSaveData.TutorialScene.TakeTablet);
        tutorialSceneObject.InteractionQuide.SetActive(!loadedSaveData.TutorialScene.InteractionQuideInteracted);

        if(Player != null)
        {
            if (SaveLoadSystem.Instance._SaveData.LastScene == SceneType.SelectionScene)
                Player.InstantMove(tutorialSceneObject.StageSelectionSP, new Vector2(0, -1));
        }
    }
    #endregion

    #region Stage Selection Scene
    [SerializeField] private StageSelectionSceneObject stageSelectionSceneObject = null;

    private void LoadStageSelectionSceneObject(PlayerController Player)
    {
        void CheckNullReferences()
        {
            if (!stageSelectionSceneObject.TutorialSP) Debug.LogError($"{name} has no spawn point from tutorial scene");
            if (!stageSelectionSceneObject.Door1SP) Debug.LogError($"{name} has no spawn point from Door 1");
        }

        CheckNullReferences();

        #region Ommiting Save data name
        SaveData loadedSaveData = SaveLoadSystem.Instance._SaveData;
        SceneType LastScene = loadedSaveData.LastScene;
        #endregion

        stageSelectionSceneObject.Door1.Activated(loadedSaveData.TutorialScene.TakeTablet);

        if (Player != null)
        {
            if (LastScene == SceneType.TutorialScene)
                Player.InstantMove(stageSelectionSceneObject.TutorialSP, new Vector2(0,1));
            else if (LastScene == SceneType.SelectionScene)
                Player.InstantMove(stageSelectionSceneObject.Door1SP, new Vector2(0, -1));
        }
    }
    #endregion
}

[Serializable] public class TutorialSceneObject
{
    public GameObject Tablet = null;
    public GameObject AwakeDialog = null;
    public GameObject InteractionQuide = null;

    [Header("Spawn Point")]
    public Transform StageSelectionSP = null;
}

[Serializable] public class StageSelectionSceneObject
{
    public Door Door1;

    [Header("Spawn Point")]
    public Transform TutorialSP = null;
    public Transform Door1SP = null;
}