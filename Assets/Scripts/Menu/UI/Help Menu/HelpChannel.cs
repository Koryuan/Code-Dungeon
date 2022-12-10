using System;
using UnityEngine;

public class HelpChannel : ScriptableObject
{
    public Action OnHelpRequested;
    public Action<HelpSettings> OnHelpInserted;
    public Action<HelpSettings[]> OnHelpInsertedMultiple;

    public void RaiseHelpDataRequested()
    {
        OnHelpRequested?.Invoke();
    }
    public void RaiseHelpInsert(HelpSettings Setting)
    {
        OnHelpInserted?.Invoke(Setting);
    }
    public void RaiseHelpInsertMultiple(HelpSettings[] Settings)
    {
        OnHelpInsertedMultiple?.Invoke(Settings);
    }
}