using Cysharp.Threading.Tasks;
using UnityEngine;

public class TriggerEnterEventScene : TriggerEnterEvent
{
    [SerializeField] private SceneName _sceneName;

    async protected override void OnPlayerEnter()
    {
        if (gameEvent.Length != 0) await GameManager.Instance.StartEvent(gameEvent);
        switch(_sceneName)
        {
            case SceneName.TutorialStage:
                SceneLoad.LoadTutorialMap();
                break;
            case SceneName.SceneSelector:
                SceneLoad.LoadSelectStage();
                break;
            default:
                break;
        }
    }
}

public enum SceneName
{
    SceneSelector,
    TutorialStage
}