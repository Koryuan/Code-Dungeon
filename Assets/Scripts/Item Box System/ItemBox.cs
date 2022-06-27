using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ItemBox : MonoBehaviour
{
    private static ItemBox box;
    private bool canControl = false;

    [Header("UI")]
    [SerializeField] private ItemBoxUI _itemBoxUI;

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
        if (!_itemBoxUI) Debug.LogError("There is no UI References");
        if (!_inventoryInput) Debug.LogError("There is no inventory input References");
        if (!_moveInput) Debug.LogError("There is no move input References");
        if (!_interactionInput) Debug.LogError("There is no interaction input References");
    }

    private void OpenInventoryBox(InputAction.CallbackContext Callback)
    {
        if (canControl) _itemBoxUI.OpenClosePanel();
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