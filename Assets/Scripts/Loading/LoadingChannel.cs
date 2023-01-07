using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channel/Loading Channel")]
public class LoadingChannel : ScriptableObject
{
    public delegate void LoadingCallback();
    public delegate void LoadingTypeCallback(LoadingType Type);
    public LoadingCallback OnRequestLoading;
    public LoadingCallback OnLoadingFinish;
    public LoadingTypeCallback OnLoadUpdated;

    public void RaiseLoadingRequest()
    {
        OnRequestLoading?.Invoke();
    }
    public void RaiseLoadingFinish()
    {
        OnLoadingFinish?.Invoke();
    }
    public void RaiseLoadUpdated(LoadingType Type)
    {
        OnLoadUpdated?.Invoke(Type);
    }
}