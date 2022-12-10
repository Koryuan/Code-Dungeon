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
            case SceneType.Print2Scene:
                LoadPrint2SceneObject(player);
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
            if (!print1SceneObject.StageSelctionSP) Debug.LogError($"Load Scene has no reference to Stage Selection Spawn Point");
            if (!print1SceneObject.Print2SP) Debug.LogError($"Load Scene has no reference to Print 2 Spawn Point");

            // Puzzle 1
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
            print1SceneObject.Machine2.PrintMessage(sceneSaveData.CodeMachine2PrintedText);

        string[] OccuredText = {"729"};
        print1SceneObject.Keypad.
            LoadKeyPad(sceneSaveData.CodeMachine2Updated ? OccuredText : null,sceneSaveData.Keypad_LastText);

        if (Player != null)
        {
            if (LastScene == SceneType.SelectionScene)
                Player.InstantMove(print1SceneObject.StageSelctionSP, new Vector2(0, 1));
            else if (LastScene == SceneType.Print2Scene)
                Player.InstantMove(print1SceneObject.Print2SP, new Vector2(0, -1));
        }
    }
    #endregion

    #region Print 2 Scene
    [SerializeField] private Print2SceneObject print2SceneObject = null;
    private void LoadPrint2SceneObject(PlayerController Player)
    {
        void CheckReferences()
        {
            // Puzzle 1
            if (!print2SceneObject.Machine1) Debug.LogError($"Load Scene has no reference to Machine 1");
            if (!print2SceneObject.Machine2) Debug.LogError($"Load Scene has no reference to Machine 2");
            if (!print2SceneObject.Door1) Debug.LogError($"Load Scene has no reference to Door 1");
            if (!print2SceneObject.NPC1) Debug.LogError($"Load Scene has no reference to NPC 1");
        }

        CheckReferences();

        #region Ommiting Save data name
        SaveData loadedSaveData = SaveLoadSystem.Instance._SaveData;
        SaveDataPrint2Scene sceneSaveData = loadedSaveData.Print2Scene;
        SceneType LastScene = loadedSaveData.LastScene;
        #endregion

        #region Puzzle 3
        print2SceneObject.Machine2.PrintMessage("Lock Release");
        print2SceneObject.Door1.Activated(sceneSaveData.Door1Open);
        if (sceneSaveData.Door1OpenOnce) print2SceneObject.NPC1.gameObject.SetActive(false);
        else if(sceneSaveData.NPCDialog > -1) print2SceneObject.NPC1.UpdateNPCEvent(sceneSaveData.NPCDialog + 1);

        if (sceneSaveData.CodeMachine1PrintedText != string.Empty) 
            print2SceneObject.Machine1.PrintMessage(sceneSaveData.CodeMachine1PrintedText);
        if (sceneSaveData.CodeMachine1InputField.IndexInArray != -1)
            print2SceneObject.Machine1.UpdateContainText(sceneSaveData.CodeMachine1InputField);

        if (Player)
        {
            if (LastScene == SceneType.Print1Scene)
                Player.InstantMove(print2SceneObject.Print1SP, new Vector2(0, -1));
        }
        #endregion

        #region Puzzle 4
        if (sceneSaveData.Puzzle4_CodeMachineIntInterected)
        {
            print2SceneObject.Puzzle4_MachineInt.OnCloseStopped();
            print2SceneObject.Puzzle4_MachineInt.ActivatePrintInteract();
        }
        if (sceneSaveData.Puzzle4_CodeMachineCharInterected)
        {
            print2SceneObject.Puzzle4_MachineChar.OnCloseStopped();
            print2SceneObject.Puzzle4_MachineChar.ActivatePrintInteract();
        }
        if (sceneSaveData.Puzzle4_CodeMachineStringInterected)
        {
            print2SceneObject.Puzzle4_MachineString.OnCloseStopped();
            print2SceneObject.Puzzle4_MachineString.ActivatePrintInteract();
        }
        if (sceneSaveData.Puzzle4_CodeMachineFloatInterected)
        {
            print2SceneObject.Puzzle4_MachineFloat.OnCloseStopped();
            print2SceneObject.Puzzle4_MachineFloat.ActivatePrintInteract();
        }
            
        foreach(string text in sceneSaveData.Puzzle4_Collective)
        {
            switch (text)
            {
                case "Code Machine - Integer - 01":
                    print2SceneObject.Puzzle4_MachineInt.PrintMessage("-1");
                    break;
                case "Code Machine - Char - 01":
                    print2SceneObject.Puzzle4_MachineChar.PrintMessage("S");
                    break;
                case "Code Machine - String - 01":
                    print2SceneObject.Puzzle4_MachineString.PrintMessage("S@@1Az3");
                    break;
                case "Code Machine - Float - 01":
                    print2SceneObject.Puzzle4_MachineFloat.PrintMessage("3.123");
                    break;
                default:
                    break;
            }
        }
        if (sceneSaveData.Puzzle4_Collective.Count > 0) 
            print2SceneObject.Puzzle4_Unlocker.UnlockManyTarget(sceneSaveData.Puzzle4_Collective.ToArray());
        print2SceneObject.Puzzle4_Door.Activated(sceneSaveData.Puzzle4_DoorOpen);
        #endregion

        Debug.Log("Everything done loaded");
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
    public Transform Print2SP;
}

[Serializable] public class Print2SceneObject
{
    [Header("Puzzle 3")]
    public CodeMachineString Machine1;
    public CodeMachine Machine2;
    public Door Door1;
    public NonPlayableCharacter NPC1;

    [Header("Puzzle 4")]
    public CodeMachine Puzzle4_MachineInt;
    public CodeMachine Puzzle4_MachineChar;
    public CodeMachine Puzzle4_MachineString;
    public CodeMachine Puzzle4_MachineFloat;
    public CollectiveStringUnlocker Puzzle4_Unlocker;
    public Door Puzzle4_Door;

    [Header("Spawn Point")]
    public Transform Print1SP;
}