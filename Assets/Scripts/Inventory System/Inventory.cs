using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private List<ItemBoxContain> itemList = new List<ItemBoxContain>();

    private ItemBoxContain currentSelection;

    private bool isClose => GameManager.Instance.CurrentState == GameState.Game_Player_State;
    private bool isOpen => GameManager.Instance.CurrentState == GameState.Game_Inventory_State;

    public event Action OnOpenInventory;
    public event Action OnCloseInventory;

    [Header("UI")]
    [SerializeField] private InventoryUI _UI;
    [SerializeField] private ItemBoxContain containerPrefab;

    [Header("Item Scroller")]
    [SerializeField] private float scrollSpeed = 0;
    [SerializeField] private bool instant = false;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform mScrollTransform;
    [SerializeField] private RectTransform _contentTransform;
    [SerializeField] private RectTransform _viewportTransform;

    public bool IsInitialize { get; private set; } = false;
    private bool dontHover { get; set; } = false;

    #region Initialization
    async public void Awake()
    {
        Instance = this;
        CheckNullReferences();

        await UniTask.WaitUntil(()=> SaveLoadSystem.Instance._SaveData != null);

        foreach (ItemBoxContain contain in itemList){
            contain.Button.OnSelectEvent += () => ChangeSelection(contain);
            contain.Button.OnHoverEvent += () => dontHover = true;
            if (contain.ItemUseable) contain.Button.onClick.AddListener(OpenYesNoBox);
        }         

        // On inventory open Event
        OnOpenInventory += UpdateItemListScrollbar;

        ResetViewport();

        IsInitialize = true;
    }
    private void CheckNullReferences()
    {
        if (!_UI) Debug.LogError("There is no UI References");

        // Scroll
        if (!_scrollRect) Debug.LogError($"{name} has no Scroll Rectangle References");
        if (!_contentTransform) Debug.LogError($"{name} has no Content Transform References");
        if (!_viewportTransform) Debug.LogError($"{name} has no Viewport Transform References");
    }
    #endregion

    public void AddNewItem(Item NewItem)
    {
        var Item = Instantiate(containerPrefab, _UI.ItemContainer);
        Item.transform.SetSiblingIndex(0);
        Item.Initialize(NewItem);

        // Update Action on Item
        Item.OnDestroy += RemoveItem;
        Item.Button.OnSelectEvent += () => ChangeSelection(Item);
        Item.Button.OnHoverEvent += () => dontHover = true;
        if (Item.ItemUseable) Item.Button.onClick.AddListener(OpenYesNoBox);

        // Add to list
        SaveLoadSystem.Instance._SaveData._ItemList.Add(NewItem);
        itemList.Add(Item);
    }
    private void RemoveItem(ItemBoxContain RemovedItemContainer, Item RemovedItem)
    {
        itemList.Remove(RemovedItemContainer);
        SaveLoadSystem.Instance._SaveData._ItemList.Remove(RemovedItem);

        // Unsubscribe
        RemovedItemContainer.OnDestroy -= RemoveItem;
    }

    private void ChangeSelection(ItemBoxContain NewSelection)
    {
        if (NewSelection != null)
        {
            if (currentSelection) currentSelection.SetHighlight(false);
            currentSelection = NewSelection;

            _UI.UpdateItemInfo(NewSelection.ItemName,NewSelection.ItemDescription);
            NewSelection.SetHighlight(true);

            //A(NewSelection);
            if (!dontHover) CenterOnItem(NewSelection.ContainerTransform);
            dontHover = false;
        }
    }

    #region Scrolling function
    #region Testing on creating myself
    private void A(ItemBoxContain NewSelection)
    {
        RectTransform selectedRectTransform = NewSelection.ContainerTransform;

        // Difference between selected and Viewport
        Vector3 selectedDifference = _viewportTransform.localPosition - selectedRectTransform.localPosition;
        Debug.Log($" Selected Difference:" +
            $" VT:{_viewportTransform.localPosition} - SRT:{selectedRectTransform.localPosition} = {selectedDifference}");

        // For each object it will be 65 for Content Transform Height
        float contentHeightDifference = (_contentTransform.rect.height - _viewportTransform.rect.height);
        Debug.Log($" Content Height Dif:" +
            $" CTH:{_contentTransform.rect.height} - VTH:{_viewportTransform.rect.height} = {contentHeightDifference}");

        // Difference between Content and Selected+Viewport difference
        float selectedPosition = (_contentTransform.rect.height - selectedDifference.y);
        Debug.Log($" Selected Position:" +
            $" CTH:{_contentTransform.rect.height} - SPH:{selectedDifference.y} = {selectedPosition}");
    }
    #endregion

    #region Ohter Scroll Function
    private void CenterOnItem(RectTransform target)
    {
        // Item is here
        var itemCenterPositionInScroll = GetWorldPointInWidget(mScrollTransform, GetWidgetWorldPoint(target));
        // But must be here
        var targetPositionInScroll = GetWorldPointInWidget(mScrollTransform, GetWidgetWorldPoint(_viewportTransform));
        // So it has to move this distance
        var difference = targetPositionInScroll - itemCenterPositionInScroll;
        difference.z = 0f;

        //clear axis data that is not enabled in the scrollrect
        if (!_scrollRect.horizontal)
        {
            difference.x = 0f;
        }
        if (!_scrollRect.vertical)
        {
            difference.y = 0f;
        }

        var normalizedDifference = new Vector2(
            difference.x / (_contentTransform.rect.size.x - mScrollTransform.rect.size.x),
            difference.y / (_contentTransform.rect.size.y - mScrollTransform.rect.size.y));

        var newNormalizedPosition = _scrollRect.normalizedPosition - normalizedDifference;
        if (_scrollRect.movementType != ScrollRect.MovementType.Unrestricted)
        {
            newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
            newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);
        }

        _scrollRect.normalizedPosition = newNormalizedPosition;
    }
    private Vector3 GetWidgetWorldPoint(RectTransform target)
    {
        //pivot position + item size has to be included
        var pivotOffset = new Vector3(
            (0.5f - target.pivot.x) * target.rect.size.x,
            (0.5f - target.pivot.y) * target.rect.size.y,
            0f);
        var localPosition = target.localPosition + pivotOffset;
        return target.parent.TransformPoint(localPosition);
    }
    private Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
    {
        return target.InverseTransformPoint(worldPoint);
    }
    private void ResetViewport()
    {
        if (_viewportTransform == null)
        {
            var mask = GetComponentInChildren<Mask>(true);
            if (mask)
            {
                _viewportTransform = mask.rectTransform;
            }
            if (_viewportTransform == null)
            {
                var mask2D = GetComponentInChildren<RectMask2D>(true);
                if (mask2D)
                {
                    _viewportTransform = mask2D.rectTransform;
                }
            }
        }
    }
    #endregion

    #region Unused Scroll Function
    private void UpdateScrollPosition(ItemBoxContain NewSelection)
    {
        // Math Calculation
        RectTransform selectedRectTransform = NewSelection.ContainerTransform;
        Vector3 selectedDifference = _viewportTransform.localPosition - selectedRectTransform.localPosition;
        float contentHeightDifference = (_contentTransform.rect.height - _viewportTransform.rect.height);

        float selectedPosition = (_contentTransform.rect.height - selectedDifference.y);
        float currentScrollRectPosition = _scrollRect.normalizedPosition.y;
        float above = currentScrollRectPosition - (selectedRectTransform.rect.height / 2) + _viewportTransform.rect.height;
        float below = currentScrollRectPosition + (selectedRectTransform.rect.height / 2);

        Debug.Log($"D: <color=red>{selectedDifference}</color>, CHD: <color=green>{contentHeightDifference}</color>, CSRP: <color=blue>{currentScrollRectPosition}</color>");
        Debug.Log($"SP: <color=red>{selectedPosition}</color>, A: <color=green>{above}</color>, B: <color=blue>{below}</color>");
        if (selectedPosition > above)
        {
            float step = selectedPosition - above;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            _scrollRect.normalizedPosition = Vector2.Lerp(_scrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.deltaTime);
            if (instant) _scrollRect.normalizedPosition = new Vector2(0, newNormalizedY);
        }
        else if (selectedPosition < below)
        {
            float step = selectedPosition - below;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            _scrollRect.normalizedPosition = Vector2.Lerp(_scrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.deltaTime);
            if (instant) _scrollRect.normalizedPosition = new Vector2(0, newNormalizedY);
        }
        Debug.Log($"Current Position: <color=green>{_scrollRect.normalizedPosition}</color>");
    }
    #endregion
    #endregion

    private async void UpdateItemListScrollbar()
    {
        await UniTask.Delay(10);
        foreach(ItemBoxContain contain in itemList)
        {
            contain.ScrollbarPopOut(_UI.ScrollbarActive);
        } _UI.UpdateViewPort(_UI.ScrollbarActive);
    }

    #region Yes / No box
    private void OpenYesNoBox()
    {
        Action onSelectYes = currentSelection.Use; onSelectYes += ClosePanel;
        Action onSelectNo = currentSelection.Select; onSelectNo += () => _UI.YesNoPanel(false);

        _UI._YesNoBox.OpenYesNo(onSelectYes, onSelectNo);
        _UI.YesNoPanel(true);
    }
    #endregion

    #region Open Close
    InputActionReference inventoryInput => InputReferences.Instance._PlayerInventoryInput;
    InputActionReference closeInput => InputReferences.Instance._MenuCloseInput;
    private void OpenClosePanel(InputAction.CallbackContext Callback)
    {
        if (isClose) OpenPanel();
        else if (isOpen) ClosePanel();
    }
    private void OpenPanel()
    {
        _UI.InventoryPanel(true);
        _UI.YesNoPanel(false);
        dontHover = true;

        if (itemList.Count > 0)
        {
            _UI.UpdateItemInfo(itemList[0].ItemName, itemList[0].ItemDescription);
            itemList[0].Select();
        } else _UI.UpdateItemInfo("", "");
        OnOpenInventory?.Invoke();
    }
    private void ClosePanel()
    {
        _UI.InventoryPanel(false);
        OnCloseInventory?.Invoke();
    }
    private void ClosePanel(InputAction.CallbackContext Callback)
    {
        if (isOpen) ClosePanel();
    }
    #endregion

    #region Enable Disable
    private async void OnEnable()
    {
        await UniTask.WaitUntil(() => InputReferences.Instance != null);
        inventoryInput.action.performed += OpenClosePanel;
        closeInput.action.performed += ClosePanel;
    }
    private void OnDisable()
    {
        inventoryInput.action.performed -= OpenClosePanel;
        closeInput.action.performed -= ClosePanel;
    }
    #endregion
}