using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Help/Help Listing")]
public class HelpList : ScriptableObject
{
    [SerializeField] private List<HelpSettings> m_helpList;

    public HelpSettings[] GetHelpList(string[] IDList)
    {
        List<HelpSettings> list = new List<HelpSettings>();
        foreach (string ID in IDList)
        {
            if (m_helpList.Count > 0 && SearchInHelpSettingList(ID, ref list)) continue;
            Debug.LogWarning($"{ID} is not added to list");
        }
        return list.ToArray();
    }
    private bool SearchInHelpSettingList(string ID, ref List<HelpSettings> HelpList)
    {
        HelpSettings searched = m_helpList.Find(x => x.Name == ID);
        if (!searched) return false;

        HelpList.Add(searched);
        return true;
    }
}