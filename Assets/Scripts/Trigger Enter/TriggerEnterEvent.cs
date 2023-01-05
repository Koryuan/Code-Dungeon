using Cysharp.Threading.Tasks;
using UnityEngine;

public class TriggerEnterEvent : TriggerEnter
{
    [Header("Event list")]
    [SerializeField] private GameEvent[] gameEvents;

    [Header("Variable")]
    [SerializeField] private bool OnEnterDisable = false;

    private AutoSaveTriggerEnter m_autoSave = null;

    private void Awake()
    {
        if (gameEvents.Length == 0) Debug.LogError($"{name} has no event to trigger");

        m_autoSave = GetComponent<AutoSaveTriggerEnter>();

        if (!m_autoSave) return;

        m_autoSave.OnDataLoaded += LoadData;
        m_autoSave.LoadData();
    }

    private void LoadData(SaveDataAuto LoadedData)
    {
        if (LoadedData.New) return;
        if (LoadedData is TriggerEnterSaveData oldData) gameObject.SetActive(oldData.ObjectActivation);
    }

    public void UpdateActivation(bool Open)
    {
        if (m_autoSave) m_autoSave.UpdateObjectActivation(Open);
        gameObject.SetActive(Open);
    }

    protected override void OnPlayerEnter()
    {
        if (gameEvents.Length == 0) return;

        GameManager.Instance.StartEvent(gameEvents).Forget();

        // After Everyting done
        AutoSaveScene.SaveObjectState(name);
        if (OnEnterDisable)
        {
            if (m_autoSave) m_autoSave.UpdateObjectActivation(false);
            gameObject.SetActive(false);
        }
    }
}