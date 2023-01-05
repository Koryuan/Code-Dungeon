using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoSaveCodeMachine : AutoSaveInterectable
{
    private CodeMachineSaveData m_saveData = new CodeMachineSaveData();

    public void AdditionalData(List<ContainReadonly> ReadonlyContains, List<ContainInputField> InputFieldContains, CompilerSaveData CompilerData)
    {
        foreach (ContainReadonly contain in ReadonlyContains) m_saveData.ReadOnlyContain.Add(contain.SaveData);
        foreach (ContainInputField contain in InputFieldContains) m_saveData.InputFieldContain.Add(contain.SaveData);
        m_saveData.CompilerData = CompilerData;
    }
    
    public void AdditionalData2(int InfiniteLoopEventOccurenceNumber)
    {
        m_saveData.InfiniteLoopOccurenceNumber = InfiniteLoopEventOccurenceNumber;
    }

    async public override void LoadData(bool CanInterect, bool CanPrint, bool CanScan, bool ObjectActive, bool AnimationActive)
    {
        m_saveData.ID = name;
        m_saveData.CanInteract = CanInterect;
        m_saveData.CanPrint = CanPrint;
        m_saveData.CanScan = CanScan;
        m_saveData.ObjectActive = ObjectActive;
        m_saveData.AnimationActive = AnimationActive;

        await UniTask.Delay(100);

        CodeMachineSaveData LoadedData = null;
        while ((LoadedData = m_saveChannel.RaiseCodeMachineSaveDataRequest(name)) == null) await UniTask.Delay(100);
        if (LoadedData.New) return;

        Debug.Log($"{name}, has old Code Machine data: Loading.....");
        m_saveData = LoadedData;
        OnDataLoaded?.Invoke(LoadedData);
    }
    public void UpdateIsBroke(bool IsBroken)
    {
        m_saveData.IsBroken = IsBroken;
        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }
    public void UpdateCompilerOccurence(CompilerSaveData NewData)
    {
        m_saveData.CompilerData = NewData;
        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }
    public void UpdateInfiniteLoopOccurenceNumber(int Number)
    {
        m_saveData.InfiniteLoopOccurenceNumber = Number;
        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }
    public void UpdateReadOnlyContain(ContainReadonlySaveData NewData)
    {
        ContainReadonlySaveData OldData = m_saveData.ReadOnlyContain.Find(x => x.Index == NewData.Index);
        if (OldData != null) OldData = NewData;
        else m_saveData.ReadOnlyContain.Add(NewData);

        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }
    public void UpdateInputFieldContain(ContainInputSaveData NewData)
    {
        ContainInputSaveData OldData = m_saveData.InputFieldContain.Find(x => x.Index == NewData.Index);
        if (OldData != null) OldData = NewData;
        else m_saveData.InputFieldContain.Add(NewData);

        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }
    public void UpdateOnClosePossible(bool Possible)
    {
        m_saveData.OnCloseCodePossible = Possible;
        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }
    public void UpdateOnMessagePrint(string[] PrintMessages)
    {
        m_saveData.PrintedMessages = PrintMessages.ToList();
        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }

    #region General Interactable object Override
    public override void UpdateCanInterect(bool CanInterect)
    {
        m_saveData.CanInteract = CanInterect;
        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }
    public override void UpdateCanPrint(bool CanPrint)
    {
        m_saveData.CanPrint = CanPrint;
        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }
    public override void UpdateCanScan(bool CanScan)
    {
        m_saveData.CanScan = CanScan;
        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }
    public override void UpdateObjectActivation(bool Active)
    {
        m_saveData.ObjectActive = Active;
        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }
    public override void UpdateAnimationActivation(bool Active)
    {
        m_saveData.AnimationActive = Active;
        m_saveChannel.RaiseCodeMachineSaveDataUpdated(m_saveData);
    }
    #endregion
}

[System.Serializable] public class CodeMachineSaveData : InteractableSaveData
{
    public bool IsBroken = false;
    public bool OnCloseCodePossible = true;
    public List<string> PrintedMessages = new List<string>();

    public int InfiniteLoopOccurenceNumber = 0;

    public CompilerSaveData CompilerData = new CompilerSaveData();
    public List<ContainReadonlySaveData> ReadOnlyContain = new List<ContainReadonlySaveData>();
    public List<ContainInputSaveData> InputFieldContain = new List<ContainInputSaveData>();
}