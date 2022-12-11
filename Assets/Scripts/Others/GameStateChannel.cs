using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channel/Game State Channel")]
public class GameStateChannel : ScriptableObject
{
    public delegate void GameStateCallback(GameState NewState);
    public event GameStateCallback OnGameStateChanged;
    public event GameStateCallback OnGameStateRequestedChange;

    public void RaiseGameStateRequested(GameState NewState)
    {
        OnGameStateRequestedChange?.Invoke(NewState);
    }
    public void RaiseGameStateChanged(GameState NewState)
    {
        OnGameStateChanged?.Invoke(NewState);
    }
}