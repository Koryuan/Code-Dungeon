using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InventorySystem : MonoBehaviour
{
    private bool canControl = false;

    [Header("UI")]
    [SerializeField] private InventoryUI inventoryUI;

    [Header("Input References")]
    [SerializeField] private InputActionReference _inventoryInput;
    [SerializeField] private InputActionReference _moveInput;
    [SerializeField] private InputActionReference _interactionInput;

    private void Awake()
    {
        CheckNullReferences();
    }
    private void CheckNullReferences()
    {
        if (!inventoryUI) Debug.LogError("There is no UI References");
        if (!_inventoryInput) Debug.LogError("There is no inventory input References");
        if (!_moveInput) Debug.LogError("There is no move input References");
        if (!_interactionInput) Debug.LogError("There is no interaction input References");
    }

    private void OpenInventoryBox(InputAction.CallbackContext Callback)
    {
        if (canControl) inventoryUI.OpenClosePanel();
    }

    private void OnEnable()
    {
        _inventoryInput.action.performed += OpenInventoryBox;
    }

    private void OnDisable()
    {
        _inventoryInput.action.performed -= OpenInventoryBox;
    }
}