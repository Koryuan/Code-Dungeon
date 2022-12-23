using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour, IPanelUI
{
    private List<ItemBoxContain> m_itemList = new List<ItemBoxContain>();

    private ItemBoxContain m_currentSelection;

    [Header("UI")]
    [SerializeField] private InventoryUI m_ui;
    [SerializeField] private ItemBoxContain m_containerPrefab;

    [Header("Channel")]
    [SerializeField] private ItemChannel m_itemChannel;
    [SerializeField] private GameStateChannel m_gameChannel;
    [SerializeField] private MenuManager m_menuManager;

    private bool m_hover { get; set; } = false;
    private bool m_canOpen { get; set; } = true;
    private bool m_canInterect { get; set; } = false;

    #region Initialization
    async public void Awake()
    {
        CheckReferences();

        m_itemChannel.OnItemInserted += AddNewItem;
        m_itemChannel.OnItemRequested += SearchItem;
        m_itemChannel.OnItemRequestRemove += RemoveRequestedItem;
        m_gameChannel.OnGameStateChanged += OnGameStateChanged;
        m_menuManager.OnMenuStateChanged += OnMenuStateChanged;

        await UniTask.Delay(100);

        Item[] LoadedItem = null;
        while ((LoadedItem = m_itemChannel.RaiseItemListRequested()) == null) await UniTask.Delay(100);
        if (LoadedItem.Length > 0) LoadItems(LoadedItem);
        else Debug.Log("Save Data, Has no any Item");
    }
    private void CheckReferences()
    {
        m_ui.CheckReferences();

        if (!m_containerPrefab) Debug.LogError("Inventory, has no container prefab to instantiate");
        if (!m_itemChannel) Debug.LogError("Inventory, has no item channel references");
    }
    #endregion

    #region Add/Remove/Search
    private void AddNewItem(Item NewItem)
    {
        var Item = Instantiate(m_containerPrefab, m_ui.ItemContainer);
        Item.transform.SetAsFirstSibling();
        Item.Create(NewItem);

        // Update Action on Item
        Item.OnContainDestroy += RemoveItem;
        Item.Button.OnSelectEvent += () => UpdateItemInfo(Item);
        Item.Button.OnHoverEvent += () => m_hover = false;
        Item.OnPressed += OpenYesNoBox;

        // Add to list
        m_itemList.Add(Item);
    }
    private void LoadItems(Item[] SavedItems)
    {
        if (SavedItems.Length == 0) return;

        foreach (Item item in SavedItems) AddNewItem(item);
        Debug.Log("Inventory, Loaded");
    }
    private void RemoveItem(ItemBoxContain RemovedItemContainer, Item RemovedItem)
    {
        m_itemList.Remove(RemovedItemContainer);
        m_itemChannel.RaiseItemRemoved(RemovedItem);

        // Unsubscribe
        RemovedItemContainer.OnContainDestroy -= RemoveItem;
    }
    private void RemoveRequestedItem(Item RequestedItem)
    {
        if (m_itemList.Count == 0) return;
        ItemBoxContain contain = m_itemList.Find(x => x.Item == RequestedItem);

        if (contain != null) contain.DestoryContain();
    }
    private Item SearchItem(string Itemname)
    {
        if (m_itemList.Count == 0) return null;
        Item SearchedItem = m_itemList.Find(x => x.ItemName == Itemname)?.Item;
        return SearchedItem;
    }
    #endregion

    #region Update
    private void UpdateItemInfo(ItemBoxContain NewSelection)
    {
        m_ui.UpdateItemInfo(NewSelection,m_hover);
        m_hover = true;
    }
    private async void UpdateItemListScrollbar()
    {
        await UniTask.Delay(10);
        foreach(ItemBoxContain contain in m_itemList) 
            contain.ScrollbarPopOut(m_ui.ScrollbarActive); 
        m_ui.UpdateViewPort(m_ui.ScrollbarActive);
    }
    private void OnGameStateChanged(GameState NewState) => m_canOpen = NewState == GameState.Game_Player_State;
    private void OnMenuStateChanged(MenuState NewState) => m_canInterect = NewState == MenuState.Inventory;
    #endregion

    #region Yes / No box
    private void OpenYesNoBox(ItemBoxContain Contain)
    {
        Action onSelectYes = Contain.Use; onSelectYes += ClosePanel;
        Action onSelectNo = Contain.Select; onSelectNo += () => m_ui.Update_Dialog(false);

        m_ui._YesNoBox.OpenYesNo(onSelectYes, onSelectNo);
        m_ui.Update_Dialog(true);
    }
    #endregion

    #region Open Close
    InputActionReference inventoryInput => InputReferences.Instance._PlayerInventoryInput;
    InputActionReference closeInput => InputReferences.Instance._Menu_Close;
    public void OpenPanel(IMenuUI LastUI)
    {
        m_menuManager.OpenMenu(this,null);

        m_ui.InventoryPanel(true);
        UpdateItemListScrollbar();
        m_hover = true;

        if (LastUI != null && LastUI is ItemBoxContain lastContain)
        {
            m_ui.UpdateItemInfo(lastContain);
            lastContain.Select();
        }
        else if (m_itemList.Count > 0)
        {
            SetAllHightlightOff();
            m_ui.UpdateItemInfo(m_itemList[0]);
            m_itemList[m_itemList.Count-1].Select();
        } else m_ui.UpdateItemInfo(null);
    }
    private void OpenPanel(InputAction.CallbackContext Callback)
    {
        if (m_canOpen) OpenPanel(null);
    }
    private void ClosePanel()
    {
        m_menuManager.CloseMenu(this);
        m_ui.InventoryPanel(false);
    }
    private void ClosePanel(InputAction.CallbackContext Callback)
    {
        if (m_canInterect) ClosePanel();
    }
    private void SetAllHightlightOff()
    {
        foreach (ItemBoxContain contain in m_itemList) contain.SetHighlight(false);
    }
    #endregion

    #region Enable Disable
    private async void OnEnable()
    {
        await UniTask.WaitUntil(() => InputReferences.Instance != null);
        inventoryInput.action.performed += OpenPanel;
        inventoryInput.action.performed += ClosePanel;
        closeInput.action.performed += ClosePanel;
    }
    private void OnDisable()
    {
        inventoryInput.action.performed -= OpenPanel;
        inventoryInput.action.performed -= ClosePanel;
        closeInput.action.performed -= ClosePanel;
    }
    private void OnDestroy()
    {
        m_itemChannel.OnItemInserted -= AddNewItem;
        m_gameChannel.OnGameStateChanged -= OnGameStateChanged;
        m_menuManager.OnMenuStateChanged -= OnMenuStateChanged;
    }
    #endregion
}