using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channel/Help Channel")]
public class HelpChannel : ScriptableObject
{
    public delegate HelpSettings[] HelpCallback();
    public event HelpCallback OnHelpListRequested;
    public event Action<HelpSettings> OnHelpInserted;
    public Func<HelpSettings, bool> OnHelpSearchRequested;

    [SerializeField] private HelpList m_helpList;

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

    public HelpSettings[] RaiseListOfHelpSetting(string[] IDList)
    {
        if (!m_helpList) return null;
        return m_helpList.GetHelpList(IDList);
    }
}