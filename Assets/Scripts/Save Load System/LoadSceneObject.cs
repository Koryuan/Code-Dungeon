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
            case SceneType.SelectionScene:
                LoadStageSelectionSceneObject(player);
                break;
            case SceneType.Print1Scene:
                LoadPrint1SceneObject(player);
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

    #region Print 1 Scene
    [SerializeField] private Print1SceneObject print1SceneObject = null;
    private void LoadPrint1SceneObject(PlayerController Player)
    {
        void CheckReferences()
        {
            // Puzzle 1
            if (!print1SceneObject.StageSelctionSP) Debug.LogError($"Load Scene has no reference to Stage Selection Spawn Point");
            if (!print1SceneObject.PrintItem1) Debug.LogError($"Load Scene has no reference to Print 1 Item");
            if (!print1SceneObject.Door1) Debug.LogError($"Load Scene has no reference to Door 1");
            if (!print1SceneObject.Machine1) Debug.LogError($"Load Scene has no reference to Machine 1");

            // Puzzle 2
            if (!print1SceneObject.PrintItem2) Debug.LogError($"Load Scene has no reference to Print 2 Item");
            if (!print1SceneObject.Door2) Debug.LogError($"Load Scene has no reference to Door 2");
            if (!print1SceneObject.Machine2) Debug.LogError($"Load Scene has no reference to Machine 2");
            if (!print1SceneObject.Keypad) Debug.LogError($"Load Scene has no reference to Machine 2");
        }

        CheckReferences();

        #region Ommiting Save data name
        SaveData loadedSaveData = SaveLoadSystem.Instance._SaveData;
        SaveDataPrint1Scene sceneSaveData = loadedSaveData.Print1Scene;
        SceneType LastScene = loadedSaveData.LastScene;
        #endregion

        // Puzzle 1
        if (sceneSaveData.TakePrint1Item) print1SceneObject.PrintItem1.ForceDisableInteraction();
        print1SceneObject.Door1.Activated(sceneSaveData.OpenDoor1);

        if (sceneSaveData.UpdateCodeMachine1Test) 
            print1SceneObject.Machine1.UnlockText(StringList.CodeMachine1_Text_Before);
        if (sceneSaveData.CodeMachine1PrintedText != string.Empty)
            print1SceneObject.Machine1.PrintMessage(sceneSaveData.CodeMachine1PrintedText);

        // Puzzle 2
        if (sceneSaveData.TreasureChest2_Opened)
        {
            print1SceneObject.PrintItem2.OpenTreasureChest(true);
            if (sceneSaveData.TreasureChest2_Taken) print1SceneObject.PrintItem2.ForceDisableInteraction();
        }
        print1SceneObject.Door2.Activated(sceneSaveData.OpenDoor2);

        if (sceneSaveData.CodeMachine2Updated)
            print1SceneObject.Machine2.UnlockText(StringList.CodeMachine2_Text_Before);
        if (sceneSaveData.CodeMachine2PrintedText != string.Empty)
            print1SceneObject.Machine1.PrintMessage(sceneSaveData.CodeMachine2PrintedText);

        string[] OccuredText = {"739"};
        print1SceneObject.Keypad.
            LoadKeyPad(sceneSaveData.TreasureChest2_Opened ? OccuredText : null,sceneSaveData.Keypad_LastText);

        if (Player != null)
        {
            if (LastScene == SceneType.SelectionScene)
                Player.InstantMove(print1SceneObject.StageSelctionSP, new Vector2(0, 1));
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
    public TreasureChest PrintItem1;
    public TreasureChest PrintItem2;

    public Door Door1;
    public Door Door2;

    public CodeMachine Machine1;
    public CodeMachine Machine2;

    public KeypadManager Keypad;

    public Transform StageSelctionSP;
}