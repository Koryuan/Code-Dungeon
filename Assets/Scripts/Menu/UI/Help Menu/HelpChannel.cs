using System;
using UnityEngine;

public class HelpChannel : ScriptableObject
{
    public delegate HelpSettings[] HelpCallback();
    public HelpCallback OnHelpRequested;
    public Action<HelpSettings> OnHelpInserted;

    public HelpSettings[] RaiseHelpDataRequested()
    {
        if (OnHelpRequested == null) return null;
        return OnHelpRequested?.Invoke();
    }
    public void RaiseHelpInsert(HelpSettings Setting)
    {
        OnHelpInserted?.Invoke(Setting);
    }
}