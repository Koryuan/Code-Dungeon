using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channel/Game Over Channel")]
public class GameOverChannel : ScriptableObject
{
    public delegate void GameOverCallback();
    public event GameOverCallback OnGameOverRequested;

    public void RaiseGameOverRequest()
    {
        OnGameOverRequested?.Invoke();
    }
}