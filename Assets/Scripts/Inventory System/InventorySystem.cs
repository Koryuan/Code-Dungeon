using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<ItemBoxContain> itemList = new List<ItemBoxContain>();

    private int currentSelection { get; set; } = -1;

    private bool isOpen => GameManager.Instance.CurrentState == GameState.Game_Inventory_State;
    private bool isClose => GameManager.Instance.CurrentState == GameState.Game_Player_State;

    public event Action OnOpenInventory;
    public event Action OnCloseInventory;

    [Header("UI")]
    [SerializeField] private InventoryUI inventoryUI;

    [Header("Input References")]
    [SerializeField] private InputActionReference _inventoryInput;
    [SerializeField] private InputActionReference _moveInput;
    [SerializeField] private InputActionReference _interactionInput;

    #region Initialization
    public void Initialize()
    {
        CheckNullReferences();
        
        // On inventory open Event
        OnOpenInventory += inventoryUI.OpenPanel;
        OnOpenInventory += () => ChangeSelection(0);
        OnOpenInventory += inventoryUI.OnCloseYesNoBox;
        OnOpenInventory += UpdateItemListScrollbar;

        // On inventory close Event
        OnCloseInventory += inventoryUI.ClosePanel;

        // On Yes/No Box
        inventoryUI._YesNoBox.onYesButton += OnSelectYes;
        inventoryUI._YesNoBox.onNoButton += OnSelectNo;
    }
    private void CheckNullReferences()
    {
        if (!inventoryUI) Debug.LogError("There is no UI References");
        if (!_inventoryInput) Debug.LogError("There is no inventory input References");
        if (!_moveInput) Debug.LogError("There is no move input References");
        if (!_interactionInput) Debug.LogError("There is no interaction input References");
    }
    #endregion

    private void ChangeSelection(int ItemNumber)
    {
        if (itemList.Count != 0)
        {
            ItemNumber = Mathf.Clamp(ItemNumber,0, itemList.Count-1);
            if (currentSelection != - 1) itemList[currentSelection].ChangeSelection(false);

            itemList[ItemNumber].ChangeSelection(true);
            inventoryUI.UpdateItemInfo(itemList[ItemNumber].ItemName, itemList[ItemNumber].ItemDescription);

            currentSelection = ItemNumber;
        }
        else
        {
            inventoryUI.UpdateItemInfo("","");
            currentSelection = -1;
        }
    }
    private async void UpdateItemListScrollbar()
    {
        await UniTask.Delay(10);
        foreach(ItemBoxContain contain in itemList)
        {
            contain.ScrollbarPopOut(inventoryUI.ScrollbarActive);
        } inventoryUI.UpdateViewPort(inventoryUI.ScrollbarActive);
    }

    #region Yes / No box
    private void OpenYesNoBox()
    {
        if (itemList[currentSelection].ItemUseable)
        {
            inventoryUI._YesNoBox.OpenYesNoBox();
            inventoryUI.OnOpenYesNoBox();
        }
    }
    private void OnSelectYes()
    {
        OnCloseInventory?.Invoke();
    }
    private void OnSelectNo()
    {
        inventoryUI._YesNoBox.CloseYesNoBox();
        inventoryUI.OnCloseYesNoBox();
    }
    #endregion

    #region Input
    private void OpenCloseBox(InputAction.CallbackContext Callback)
    {
        if (isOpen && inventoryUI.isOpen) OnCloseInventory?.Invoke();
        else if (isClose && !inventoryUI.isOpen) OnOpenInventory?.Invoke();
    }
    private void ChangeSelectionInput(InputAction.CallbackContext Callback)
    {
        if (isOpen && inventoryUI.isOpen && !inventoryUI._YesNoBox.IsOpen)
        {
            float movePosition = _moveInput.action.ReadValue<float>();
            if (movePosition > 0) ChangeSelection(currentSelection + 1);
            else ChangeSelection(currentSelection - 1);
        }
        else if (isOpen && inventoryUI.isOpen && inventoryUI._YesNoBox.IsOpen) inventoryUI._YesNoBox.ChangeCurrentButton();
    }
    private void SelectInput(InputAction.CallbackContext Callback)
    {
        if (isOpen && inventoryUI.isOpen && !inventoryUI._YesNoBox.IsOpen) OpenYesNoBox();
        else if (isOpen && inventoryUI.isOpen && inventoryUI._YesNoBox.IsOpen) inventoryUI._YesNoBox.SelectButton();
    }
    #endregion

    private void OnEnable()
    {
        _inventoryInput.action.performed += OpenCloseBox;
        _moveInput.action.performed += ChangeSelectionInput;
        _interactionInput.action.performed += SelectInput;
    }
    private void OnDisable()
    {
        _inventoryInput.action.performed -= OpenCloseBox;
        _moveInput.action.performed -= ChangeSelectionInput;
        _interactionInput.action.performed -= SelectInput;
    }
}