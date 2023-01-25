using Cysharp.Threading.Tasks;
using UnityEngine;

public class SaveChanneler : MonoBehaviour
{
    [SerializeField] private HelpChannel m_helpChannel;
    [SerializeField] private ItemChannel m_itemChannel;
    [SerializeField] private SaveChannel m_saveChannel;

    async private void Awake()
    {
        await UniTask.WaitUntil(() => SaveLoadSystem.Instance && SaveLoadSystem.Instance._SaveData != null);

        // Help Channel
        m_helpChannel.OnHelpInserted += OnHelpInsert;
        m_helpChannel.OnHelpListRequested += OnRequestHelpList;

        // Item Channel
        m_itemChannel.OnItemInserted += OnItemInsert;
        m_itemChannel.OnItemListRequested += OnRequestItemList;
        m_itemChannel.OnItemRemoved += RemoveItemFromList;

        // Object
        m_saveChannel.OnObjectSaveDataRequest += GetObjectSaveData;
        m_saveChannel.OnObjectSaveDataUpdated += UpdateObjectSaveData;

        // NPC
        m_saveChannel.OnNPCSaveDataRequest += GetNPCSaveData;
        m_saveChannel.OnNPCSaveDataUpdated += UpdateNPCSaveData;

        // Treasure Chest
        m_saveChannel.OnTreasureChestSaveDataRequest += GetTreasureChestSaveData;
        m_saveChannel.OnTreasureChestSaveDataUpdated += UpdateTreasureChestSaveData;

        // Keypad
        m_saveChannel.OnKeypadSaveDataRequest += GetKeypadSaveData;
        m_saveChannel.OnKeypadSaveDataUpdated += UpdateKeypadSaveData;

        // Door
        m_saveChannel.OnDoorSaveDataRequest += GetDoorSaveData;
        m_saveChannel.OnDoorSaveDataUpdated += UpdateDoorSaveData;

        // Code Machine
        m_saveChannel.OnCodeMachineSaveDataRequest += GetCodeMachineSaveData;
        m_saveChannel.OnCodeMachineSaveDataUpdated += UpdateCodeMachineSaveData;

        // String Unlocker
        m_saveChannel.OnStringUnlockerSaveDataRequest += GetStringUnlockerSaveData;
        m_saveChannel.OnStringUnlockerSaveDataUpdated += UpdateStringUnlockerSaveData;

        // Trigger Enter
        m_saveChannel.OnTriggerEnterSaveDataRequest += GetTriggerEnterSaveData;
        m_saveChannel.OnTriggerEnterSaveDataUpdated += UpdateTriggerEnterSaveData;

        // On Enable Event
        m_saveChannel.On_OnEnableEventSaveDataRequest += GetOnEnableEventSaveData;
        m_saveChannel.On_OnEnableEventSaveDataUpdated += UpdateOnEnableEventSaveData;
    }

    #region Item
    private void OnItemInsert(Item NewItem)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        if (SaveLoadSystem.Instance._SaveData._ItemList.Contains(NewItem.name)) return;
        SaveLoadSystem.Instance._SaveData._ItemList.Add(NewItem.ItemName);
    }
    private void RemoveItemFromList(Item RemovedItem) => SaveLoadSystem.Instance._SaveData._ItemList.Remove(RemovedItem.ItemName);
    private Item[] OnRequestItemList()
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        //Debug.Log($"{name}, send item list data");
        return m_itemChannel.RaiseListOfItem(SaveLoadSystem.Instance._SaveData._ItemList.ToArray());
    }
    #endregion

    #region Help
    private void OnHelpInsert(HelpSettings NewSetting)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        if (SaveLoadSystem.Instance._SaveData.HelpList.Contains(NewSetting.Name)) return ;
        SaveLoadSystem.Instance._SaveData.HelpList.Add(NewSetting.Name);
    }
    private HelpSettings[] OnRequestHelpList()
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        //Debug.Log($"{name}, send help list data");
        return m_helpChannel.RaiseListOfHelpSetting(SaveLoadSystem.Instance._SaveData.HelpList.ToArray());
    }
    #endregion

    #region Object
    // Save/Load Scene Object
    private InteractableSaveData GetObjectSaveData(string ID)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        InteractableSaveData OldData = SaveLoadSystem.Instance._SaveData.GetObjectSaveData(ID);
        if (OldData == null) return new InteractableSaveData();
        return OldData;
    }
    private void UpdateObjectSaveData(InteractableSaveData NewData)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        SaveLoadSystem.Instance._SaveData.AddObjectSaveData(NewData);
    }
    #endregion

    #region NPC
    // Save/Load Scene NPC
    private NPCSaveData GetNPCSaveData(string ID)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        NPCSaveData OldData = SaveLoadSystem.Instance._SaveData.GetNPCSaveData(ID);
        if (OldData == null) return new NPCSaveData();
        return OldData;
    }
    private void UpdateNPCSaveData(NPCSaveData NewData)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        SaveLoadSystem.Instance._SaveData.AddNPCSaveData(NewData);
    }
    #endregion

    #region Treasure Chest
    // Save/Load Scene Treasure Chest
    private TreasureChestSaveData GetTreasureChestSaveData(string ID)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        TreasureChestSaveData OldData = SaveLoadSystem.Instance._SaveData.GetTreasureChestSaveData(ID);
        if (OldData == null) return new TreasureChestSaveData();
        return OldData;
    }
    private void UpdateTreasureChestSaveData(TreasureChestSaveData NewData)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        SaveLoadSystem.Instance._SaveData.AddTreasureChestSaveData(NewData);
    }
    #endregion

    #region Keypad
    // Save/Load Scene Keypad
    private KeypadSaveData GetKeypadSaveData(string ID)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        KeypadSaveData OldData = SaveLoadSystem.Instance._SaveData.GetKeypadSaveData(ID);
        if (OldData == null) return new KeypadSaveData();
        return OldData;
    }
    private void UpdateKeypadSaveData(KeypadSaveData NewData)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        SaveLoadSystem.Instance._SaveData.AddKeypadSaveData(NewData);
    }
    #endregion

    #region Door
    // Save/Load Scene Door
    private DoorSaveData GetDoorSaveData(string ID)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        DoorSaveData OldData = SaveLoadSystem.Instance._SaveData.GetDoorSaveData(ID);
        if (OldData == null) return new DoorSaveData();
        Debug.Log($"{ID} has old Door Data, Return Old Door Data");
        return OldData;
    }
    private void UpdateDoorSaveData(DoorSaveData NewData)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        SaveLoadSystem.Instance._SaveData.AddDoorSaveData(NewData);
    }
    #endregion

    #region Code Machine
    private CodeMachineSaveData GetCodeMachineSaveData(string ID)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        CodeMachineSaveData OldData = SaveLoadSystem.Instance._SaveData.GetCodeMachineSaveData(ID);
        if (OldData == null) return new CodeMachineSaveData();
        return OldData;
    }
    private void UpdateCodeMachineSaveData(CodeMachineSaveData NewData)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        SaveLoadSystem.Instance._SaveData.AddCodeMachineSaveData(NewData);
    }
    #endregion

    #region String Unlocker
    private StringUnlockerSaveData GetStringUnlockerSaveData(string ID)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        StringUnlockerSaveData OldData = SaveLoadSystem.Instance._SaveData.GetStringUnlockerSaveData(ID);
        if (OldData == null) return new StringUnlockerSaveData();
        return OldData;
    }
    private void UpdateStringUnlockerSaveData(StringUnlockerSaveData NewData)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        SaveLoadSystem.Instance._SaveData.AddStringUnlockerSaveData(NewData);
    }
    #endregion

    #region Trigger Enter
    private TriggerEnterSaveData GetTriggerEnterSaveData(string ID)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        TriggerEnterSaveData OldData = SaveLoadSystem.Instance._SaveData.GetTriggerEnterSaveData(ID);
        if (OldData == null) return new TriggerEnterSaveData();
        return OldData;
    }
    private void UpdateTriggerEnterSaveData(TriggerEnterSaveData NewData)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        SaveLoadSystem.Instance._SaveData.AddTriggerEnterSaveData(NewData);
    }
    #endregion

    #region On Enable Event
    private OnEnableEventSaveData GetOnEnableEventSaveData(string ID)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        OnEnableEventSaveData OldData = SaveLoadSystem.Instance._SaveData.GetOnEnableEventSaveData(ID);
        if (OldData == null) return new OnEnableEventSaveData();
        return OldData;
    }
    private void UpdateOnEnableEventSaveData(OnEnableEventSaveData NewData)
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        SaveLoadSystem.Instance._SaveData.AddOnEnableEventSaveData(NewData);
    }
    #endregion

    private void OnDestroy()
    {
        // Help
        m_helpChannel.OnHelpInserted -= OnHelpInsert;
        m_helpChannel.OnHelpListRequested -= OnRequestHelpList;

        // Item
        m_itemChannel.OnItemInserted -= OnItemInsert;
        m_itemChannel.OnItemListRequested -= OnRequestItemList;
        m_itemChannel.OnItemRemoved -= RemoveItemFromList;

        // Object
        m_saveChannel.OnObjectSaveDataRequest -= GetObjectSaveData;
        m_saveChannel.OnObjectSaveDataUpdated -= UpdateObjectSaveData;

        // NPC
        m_saveChannel.OnNPCSaveDataRequest -= GetNPCSaveData;
        m_saveChannel.OnNPCSaveDataUpdated -= UpdateNPCSaveData;

        // Treasure Chest
        m_saveChannel.OnTreasureChestSaveDataRequest -= GetTreasureChestSaveData;
        m_saveChannel.OnTreasureChestSaveDataUpdated -= UpdateTreasureChestSaveData;

        // Keypad
        m_saveChannel.OnKeypadSaveDataRequest -= GetKeypadSaveData;
        m_saveChannel.OnKeypadSaveDataUpdated -= UpdateKeypadSaveData;

        // Door
        m_saveChannel.OnDoorSaveDataRequest -= GetDoorSaveData;
        m_saveChannel.OnDoorSaveDataUpdated -= UpdateDoorSaveData;

        // Code Machine
        m_saveChannel.OnCodeMachineSaveDataRequest -= GetCodeMachineSaveData;
        m_saveChannel.OnCodeMachineSaveDataUpdated -= UpdateCodeMachineSaveData;

        // String Unlocker
        m_saveChannel.OnStringUnlockerSaveDataRequest -= GetStringUnlockerSaveData;
        m_saveChannel.OnStringUnlockerSaveDataUpdated -= UpdateStringUnlockerSaveData;

        // Trigger Enter
        m_saveChannel.OnTriggerEnterSaveDataRequest -= GetTriggerEnterSaveData;
        m_saveChannel.OnTriggerEnterSaveDataUpdated -= UpdateTriggerEnterSaveData;

        // On Enable Event
        m_saveChannel.On_OnEnableEventSaveDataRequest -= GetOnEnableEventSaveData;
        m_saveChannel.On_OnEnableEventSaveDataUpdated -= UpdateOnEnableEventSaveData;
    }
}