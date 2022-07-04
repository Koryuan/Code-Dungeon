using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterEvent : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private GameEvent[] gameEvent;

    [Header("Bool")]
    [SerializeField] private bool onTriggerDisable = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (gameEvent.Length != 0) GameManager.Instance.StartEvent(gameEvent);
            else Debug.LogError($"{name} has no event to start");
            if (onTriggerDisable) gameObject.SetActive(false);
        }
    }
}