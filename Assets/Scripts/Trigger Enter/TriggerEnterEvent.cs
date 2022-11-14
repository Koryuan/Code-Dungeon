using Cysharp.Threading.Tasks;
using UnityEngine;

public class TriggerEnterEvent : TriggerEnter
{
    [Header("Event list")]
    [SerializeField] private GameEvent[] gameEvents;

    [Header("Variable")]
    [SerializeField] private bool OnEnterDisable = false;

    private void Awake()
    {
        if (gameEvents.Length == 0) Debug.Log($"{name} has no event to trigger");
    }

    protected override void OnPlayerEnter()
    {
        if (gameEvents.Length == 0) return;

        GameManager.Instance.StartEvent(gameEvents).Forget();

        // After Everyting done
        AutoSaveScene.SaveObjectState(name);
        if (OnEnterDisable) gameObject.SetActive(false);
    }
}