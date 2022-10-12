using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterEvent : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] protected GameEvent[] gameEvent;

    [Header("Bool")]
    [SerializeField] protected bool onTriggerDisable = false;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") OnPlayerEnter();
    }

    protected virtual void OnPlayerEnter()
    {
        if (gameEvent.Length != 0) GameManager.Instance.StartEvent(gameEvent).Forget();
        else Debug.LogError($"{name} has no event to start");
        if (onTriggerDisable)
        {
            AutoSaveScene.SaveObjectState(gameObject.name);
            gameObject.SetActive(false);
        }
    }
}