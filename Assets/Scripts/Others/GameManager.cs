using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Freeze Priority

    [Header("Input References")]
    [SerializeField] private InputActionAsset playerActionMap;

    [Header("System References")]
    [SerializeField] private DialogBox dialogSystem;
    [SerializeField] private PlayerController playerSystem;

    [Header("Object Not Allow Before Initialize")]
    [SerializeField] private List<GameObject> notAllowObject = new List<GameObject>();

    async private void Awake()
    {
        await UniTask.WaitUntil(()=> playerSystem.isInitialize);
        dialogSystem.OnDialogBoxOpen += OnDialogBoxOpen;
        dialogSystem.OnDialogBoxClose += OnDialogBoxClose;

        foreach (GameObject notActiveObject in notAllowObject) notActiveObject.SetActive(true);
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