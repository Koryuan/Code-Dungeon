[System.Serializable] public class GameEventWithOccurence
{
    [UnityEngine.SerializeField] private string m_name;
    [UnityEngine.SerializeField] private int m_numberOccurence;
    [UnityEngine.SerializeField] private GameEvent[] m_eventList;

    public GameEvent[] Occur()
    {
        if (m_numberOccurence == 0) return null;

        if (m_numberOccurence != -1) m_numberOccurence -= 1;
        return m_eventList;
    }
    public int GetEventLength => m_eventList.Length;
    public int GetCurrentOccurenceNumber => m_numberOccurence;

    public void LoadOccurenceNumber(int Number) => m_numberOccurence = Number;
}