using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channel/Game State Channel")]
public class GameStateChannel : ScriptableObject
{
    public delegate void GameStateCallback(GameState NewState);
    public event GameStateCallback OnGameStateChanged;
    public event GameStateCallback OnGameStateRequestedChange;
    public event GameStateCallback OnGameStateRequestedRemove;
    public event System.Action<GameEvent[]> OnGameEventPassed;

    public delegate GameState GameStateRequest();
    public event GameStateRequest OnGameStateRequested;

    public void RaiseGameStateRequestChange(GameState NewState)
    {
        OnGameStateRequestedChange?.Invoke(NewState);
    }
    public void RaiseGameStateRemoveState(GameState OldState)
    {
        OnGameStateRequestedRemove?.Invoke(OldState);
    }
    public void RaiseGameStateChanged(GameState NewState)
    {
        OnGameStateChanged?.Invoke(NewState);
    }
    public void RaiseGameEventPassed(GameEvent[] EventList)
    {
        OnGameEventPassed?.Invoke(EventList);
    }
    public GameState RaiseGamestateRequested()
    {
        if (OnGameStateRequested == null) return GameState.None;
        return OnGameStateRequested.Invoke();
    }
}