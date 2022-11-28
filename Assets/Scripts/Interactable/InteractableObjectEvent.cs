using Cysharp.Threading.Tasks;
using UnityEngine;

public class InteractableObjectEvent : InteractableTarget
{
    [Header("Game Event")]
    [SerializeField] private GameEvent[] eventList;

    async protected override UniTask Interaction()
    {
        await GameManager.Instance.StartEvent(eventList);
    }

    protected override UniTask PrintInteraction() => throw new System.NotImplementedException();

    protected override UniTask ScanInteraction() => throw new System.NotImplementedException();
}