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
        PlayerController player = GameManager.Instance.Player;
        if (player != null && SaveLoadSystem.Instance.LoadFromSaveData)
        {
            player.InstantMove(SaveLoadSystem.Instance._SaveData.PlayerLastPosition);
            SaveLoadSystem.Instance.LoadFromSaveData = false;
        }

        switch (thisScene)
        {
            case SceneType.TutorialScene:
                LoadTutorialSceneObject(player);
                break;
            case SceneType.SelectStageScene:
                LoadStageSelectionSceneObject(player);
                break;
            case SceneType.Print1Scene:
                LoadPrint1SceneObject(player);
                break;
            case SceneType.Print2Scene:
                LoadPrint2SceneObject(player);
                break;
            case SceneType.SelectionScene:
                LoadPlayerPositionInSelectionScene(player);
                break;
            default:
                break;
        }

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
            if (SaveLoadSystem.Instance._SaveData.LastScene == SceneType.SelectStageScene)
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
            else if (LastScene == SceneType.SelectStageScene)
                Player.InstantMove(stageSelectionSceneObject.Door1SP, new Vector2(0, -1));
        }
    }
    #endregion

    #region Print 1 Scene
    [SerializeField] private Print1SceneObject print1SceneObject = null;
    private void LoadPrint1SceneObject(PlayerController Player)
    {
        void CheckReferences()
        {
            if (!print1SceneObject.StageSelctionSP) Debug.LogError($"Load Scene has no reference to Stage Selection Spawn Point");
            if (!print1SceneObject.Print2SP) Debug.LogError($"Load Scene has no reference to Print 2 Spawn Point");
        }

        CheckReferences();

        SaveData loadedSaveData = SaveLoadSystem.Instance._SaveData;
        SceneType LastScene = loadedSaveData.LastScene;

        if (Player != null)
        {
            if (LastScene == SceneType.SelectStageScene)
                Player.InstantMove(print1SceneObject.StageSelctionSP, new Vector2(0, 1));
            else if (LastScene == SceneType.Print2Scene)
                Player.InstantMove(print1SceneObject.Print2SP, new Vector2(0, -1));
        }
    }
    #endregion

    #region Print 2 Scene
    [SerializeField] private Print2SceneSP print2SceneObject = null;
    private void LoadPrint2SceneObject(PlayerController Player)
    {
        #region Ommiting Save data name
        SaveData loadedSaveData = SaveLoadSystem.Instance._SaveData;
        SceneType LastScene = loadedSaveData.LastScene;
        #endregion

        if (Player)
        {
            if (LastScene == SceneType.Print1Scene)
                Player.InstantMove(print2SceneObject.Print1SP, new Vector2(0, -1));
            else if (LastScene == SceneType.SelectionScene)
                Player.InstantMove(print2SceneObject.SelectionSP, new Vector2(0,-1));
        }

        Debug.Log("Everything done loaded");
    }
    #endregion

    #region Selection Scene
    [SerializeField] private SelectionSceneSP m_selectionSceneSP;
    private void LoadPlayerPositionInSelectionScene(PlayerController Player)
    {
        SaveData loadedSaveData = SaveLoadSystem.Instance._SaveData;
        SceneType LastScene = loadedSaveData.LastScene;

        if (Player != null)
        {
            if (LastScene == SceneType.Print2Scene)
                Player.InstantMove(m_selectionSceneSP.Print2SP, new Vector2(0, 1));
            else if (LastScene == SceneType.SelectStageScene)
                Player.InstantMove(m_selectionSceneSP.SelectStageSP, new Vector2(0, -1));
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

[Serializable] public class Print1SceneObject
{
    public Transform StageSelctionSP;
    public Transform Print2SP;
}

[Serializable] public class Print2SceneSP
{
    public Transform Print1SP;
    public Transform SelectionSP;
}

[Serializable] public class SelectionSceneSP
{
    public Transform Print2SP;
    public Transform SelectStageSP;
}