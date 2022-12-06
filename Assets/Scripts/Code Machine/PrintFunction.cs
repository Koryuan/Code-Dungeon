﻿using Cysharp.Threading.Tasks;
using UnityEngine;

public class PrintFunction : MonoBehaviour
{
    [SerializeField] private GameEvent[] eventList;

    async public virtual UniTask Activate(bool Correct = false)
    {
        if (eventList.Length > 0)
        {
            await GameManager.Instance.StartEvent(eventList);
        }
    }
}