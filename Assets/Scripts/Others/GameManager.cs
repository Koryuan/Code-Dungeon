using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    // Public References
    public bool CanOpenItemBox => false;
    public bool isInitialize { get; private set; } = false;

    // UI Priority

    [Header("Input References")]
    [SerializeField] private InputActionAsset playerActionMap;

    [Header("System References")]
    [SerializeField] private DialogBox dialogSystem;
    [SerializeField] private PlayerController playerSystem;

    [Header("Object After Initialize")]
    [SerializeField] private List<GameObject> objectAfterInit = new List<GameObject>();

    async private void Awake()
    {
        await UniTask.WaitUntil(()=> playerSystem.isInitialize);
        dialogSystem.OnDialogBoxOpen += OnDialogBoxOpen;
        dialogSystem.OnDialogBoxClose += OnDialogBoxClose;

        foreach (GameObject notActiveObject in objectAfterInit) notActiveObject.SetActive(true);
        isInitialize = true;
    }

    private void OnDialogBoxOpen()
    {
        playerSystem.FreezePlayer();
    }
    private void OnDialogBoxClose()
    {
        playerSystem.UnFreezePlayer();
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