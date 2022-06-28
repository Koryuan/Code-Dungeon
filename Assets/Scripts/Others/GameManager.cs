using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Public References
    public bool CanOpenItemBox => false;
    public bool isInitialize { get; private set; } = false;
    public GameState CurrentState { get; private set; } = GameState.PlayerControl;

    // UI Priority

    [Header("Input References")]
    [SerializeField] private InputActionAsset playerActionMap;

    [Header("System References")]
    [SerializeField] private DialogBox dialogSystem;

    [Header("Object After Initialize")]
    [SerializeField] private List<GameObject> objectAfterInit = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        dialogSystem.OnDialogBoxOpen += OnDialogBoxOpen;
        dialogSystem.OnDialogBoxClose += OnDialogBoxClose;

        foreach (GameObject notActiveObject in objectAfterInit) notActiveObject.SetActive(true);
        isInitialize = true;
    }

    private void OnDialogBoxOpen()
    {
        CurrentState = GameState.DialogBoxOpen;
    }
    private void OnDialogBoxClose()
    {
        CurrentState = GameState.PlayerControl;
    }

    #region Enable Disable
    private void OnEnable()
    {
        playerActionMap.Enable();
    }

    private void OnDisable()
    {
        playerActionMap.Disable();
    }
    #endregion
}

public enum GameState
{
    PlayerControl,
    DialogBoxOpen,
    InVentoryOpen,
    PauseGame,
}