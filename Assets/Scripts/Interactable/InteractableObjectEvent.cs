using Cysharp.Threading.Tasks;
using UnityEngine;

public class InteractableObjectEvent : InteractableTarget
{
    [SerializeField] protected GameEvent[] eventList;

    async protected override UniTask Interaction()
    {
        await GameManager.Instance.StartEvent(eventList);
    }
}