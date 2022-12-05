using Cysharp.Threading.Tasks;
using UnityEngine;

public class PrintFunctionDouble : PrintFunction
{
    [Header("Ignore the one above")]
    [SerializeField] private GameEvent[] onCorrect;
    [SerializeField] private GameEvent[] onFalse;

    async public override UniTask Activate(bool Correct = false)
    {
        if (!GameManager.Instance) return;
        if (Correct && onCorrect.Length > 0)
        {
            await GameManager.Instance.StartEvent(onCorrect);
        }
        else if (onFalse.Length > 0)
        {
            await GameManager.Instance.StartEvent(onFalse);
        }
    }
}