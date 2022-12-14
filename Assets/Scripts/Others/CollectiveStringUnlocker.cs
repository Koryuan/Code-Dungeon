using System;
using UnityEngine;

public class CollectiveStringUnlocker : MonoBehaviour
{
    [Serializable] private class StringCollection
    {
        [SerializeField] private string m_stringTarget;
        public bool Unlocked { get; private set; } = false;

        public bool UnlockString(string Input)
        {
            if (Unlocked || Input != m_stringTarget) return false;
            return Unlocked = true;
        }
    }

    [SerializeField] StringCollection[] m_stringTargetList;
    [SerializeField] GameEvent[] m_onAllUnlock;

    public bool CheckStatus()
    {
        foreach(StringCollection target in m_stringTargetList)
        {
            if (!target.Unlocked) return false;
        }
        return true;
    }
    public bool UnlockTarget(string Input)
    {
        foreach(StringCollection target in m_stringTargetList)
        {
            if (target.UnlockString(Input)) return true;
        }
        return false;
    }
    async public void UnlockTargetWithEffect(string Input)
    {
        if (!UnlockTarget(Input) || !CheckStatus() || m_onAllUnlock.Length == 0) return;
        await GameManager.Instance.StartEvent(m_onAllUnlock);
    }
    public void UnlockManyTarget(string[] Input)
    {
        foreach (string t in Input) UnlockTarget(t);
    }
}