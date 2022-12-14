using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channel/Save Channel")]
public class SaveChannel : ScriptableObject
{
    // Interactable Object Save Data
    public event Func<string, InteractableSaveData> OnObjectSaveDataRequest;
    public event Action<InteractableSaveData> OnObjectSaveDataUpdated;

    public InteractableSaveData RaiseObjectSaveDataRequest(string ID)
    {
        if (OnObjectSaveDataRequest == null) return null;
        return OnObjectSaveDataRequest?.Invoke(ID);
    }
    public void RaiseObjectSaveDataUpdated(InteractableSaveData NewSaveData)
    {
        OnObjectSaveDataUpdated?.Invoke(NewSaveData);
    }

    // NPC Save Data
    public event Func<string, NPCSaveData> OnNPCSaveDataRequest;
    public event Action<NPCSaveData> OnNPCSaveDataUpdated;

    public NPCSaveData RaiseNPCSaveDataRequest(string ID)
    {
        if (OnNPCSaveDataRequest == null) return null;
        return OnNPCSaveDataRequest?.Invoke(ID);
    }
    public void RaiseNPCSaveDataUpdated(NPCSaveData NewSaveData)
    {
        OnNPCSaveDataUpdated?.Invoke(NewSaveData);
    }

    // Treasure Chest
    public event Func<string, TreasureChestSaveData> OnTreasureChestSaveDataRequest;
    public event Action<TreasureChestSaveData> OnTreasureChestSaveDataUpdated;

    public TreasureChestSaveData RaiseTreasureChestSaveDataRequest(string ID)
    {
        if (OnTreasureChestSaveDataRequest == null) return null;
        return OnTreasureChestSaveDataRequest?.Invoke(ID);
    }
    public void RaiseTreasureChestSaveDataUpdated(TreasureChestSaveData NewSaveData)
    {
        OnTreasureChestSaveDataUpdated?.Invoke(NewSaveData);
    }

    // Keypad
    public event Func<string, KeypadSaveData> OnKeypadSaveDataRequest;
    public event Action<KeypadSaveData> OnKeypadSaveDataUpdated;

    public KeypadSaveData RaiseKeypadSaveDataRequest(string ID)
    {
        if (OnKeypadSaveDataRequest == null) return null;
        return OnKeypadSaveDataRequest?.Invoke(ID);
    }
    public void RaiseKeypadSaveDataUpdated(KeypadSaveData NewSaveData)
    {
        OnKeypadSaveDataUpdated?.Invoke(NewSaveData);
    }

    // Door
    public event Func<string, DoorSaveData> OnDoorSaveDataRequest;
    public event Action<DoorSaveData> OnDoorSaveDataUpdated;

    public DoorSaveData RaiseDoorSaveDataRequest(string ID)
    {
        if (OnDoorSaveDataRequest == null) return null;
        return OnDoorSaveDataRequest?.Invoke(ID);
    }
    public void RaiseDoorSaveDataUpdated(DoorSaveData NewSaveData)
    {
        OnDoorSaveDataUpdated?.Invoke(NewSaveData);
    }

    // Code Machine
    public event Func<string, CodeMachineSaveData> OnCodeMachineSaveDataRequest;
    public event Action<CodeMachineSaveData> OnCodeMachineSaveDataUpdated;

    public CodeMachineSaveData RaiseCodeMachineSaveDataRequest(string ID)
    {
        if (OnCodeMachineSaveDataRequest == null) return null;
        return OnCodeMachineSaveDataRequest?.Invoke(ID);
    }
    public void RaiseCodeMachineSaveDataUpdated(CodeMachineSaveData NewSaveData)
    {
        OnCodeMachineSaveDataUpdated?.Invoke(NewSaveData);
    }
}