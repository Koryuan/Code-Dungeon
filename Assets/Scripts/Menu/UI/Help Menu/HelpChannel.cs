using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channel/Help Channel")]
public class HelpChannel : ScriptableObject
{
    public delegate HelpSettings[] HelpCallback();
    public event HelpCallback OnHelpListRequested;
    public event Action<HelpSettings> OnHelpInserted;
    public Func<HelpSettings, bool> OnHelpSearchRequested;

    public HelpSettings[] RaiseHelpListRequested()
    {
        if (OnHelpListRequested == null) return null;
        return OnHelpListRequested?.Invoke();
    }
    public void RaiseHelpInsert(HelpSettings Setting)
    {
        OnHelpInserted?.Invoke(Setting);
    }
    public bool RaiseHelpSearchRequested(HelpSettings SearchedSetting)
    {
        if (OnHelpSearchRequested == null) return false;
        return OnHelpSearchRequested.Invoke(SearchedSetting);
    }
}