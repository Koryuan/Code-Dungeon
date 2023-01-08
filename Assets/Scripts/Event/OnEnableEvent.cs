using Cysharp.Threading.Tasks;
using UnityEngine;

public class OnEnableEvent : MonoBehaviour
{
    //private int m_currentIndex = -1;
    private bool m_initialized = false;
    private bool m_objectEnabled = false;
    private AutoSaveOnEnableEvent m_autoSave = null;

    [Header("Initial Value")]
    //[SerializeField] private int m_startIndex = 0;
    [SerializeField] private bool m_startEnable = true;

    [Header("Event List")]
    [SerializeField] private GameEvent[] m_event;

    [Header("After Value")]
    [SerializeField] private bool m_disableAfter = false;

    [Header("Channel")]
    [SerializeField] private GameStateChannel m_gameChannel;

    private void Awake()
    {
        m_autoSave = GetComponent<AutoSaveOnEnableEvent>();
        //m_currentIndex = m_startIndex;
        m_objectEnabled = m_startEnable;

        if (m_autoSave)
        {
            m_autoSave.OnDataLoaded += OnDataLoaded;
            m_autoSave.LoadData(m_startEnable);
        }
    }

    private void OnEnable()
    {
        if (!m_initialized || !m_objectEnabled) return;
        StartEvent();
    }

    async private void StartEvent()
    {
        if (!m_objectEnabled) return;
        while (m_gameChannel.RaiseGamestateRequested() != GameState.Game_Player_State) await UniTask.Delay(10);
        if (m_event.Length > 0) m_gameChannel.RaiseGameEventPassed(m_event);
        if (m_disableAfter)
        {
            gameObject.SetActive(false);
            m_autoSave.UpdateEnable(m_objectEnabled = false);
        }
    }

    private void OnDataLoaded(SaveDataAuto LoadedData)
    {
        if (LoadedData != null && !LoadedData.New && LoadedData is OnEnableEventSaveData oldData)
        {
            //m_currentIndex = oldData.CurrentIndex;
            gameObject.SetActive(m_objectEnabled = oldData.Enabled);
        }

        m_initialized = true;
        if (m_objectEnabled) StartEvent();
        else gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (m_autoSave) m_autoSave.OnDataLoaded -= OnDataLoaded;
    }
}