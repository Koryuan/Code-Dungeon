using UnityEngine;

public class TriggerEnterScene : TriggerEnter
{
    [SerializeField] private SceneType _sceneType;
    [SerializeField] private LoadingChannel m_loadingChannel;

    protected override void OnPlayerEnter()
    {
        if (m_loadingChannel) m_loadingChannel.RaiseLoadingRequest();
        switch(_sceneType)
        {
            case SceneType.TutorialScene:
                SceneLoad.LoadTutorialMap();
                break;
            case SceneType.SelectStageScene:
                SceneLoad.LoadSelectStage();
                break;
            case SceneType.Print1Scene:
                SceneLoad.LoadPrint1Stage();
                break;
            case SceneType.Print2Scene:
                SceneLoad.LoadPrint2Stage();
                break;
            case SceneType.SelectionScene:
                SceneLoad.LoadSelectionStage();
                break;
            case SceneType.LoopScene:
                SceneLoad.LoadLoopStage();
                break;
            default:
                break;
        }
    }
}