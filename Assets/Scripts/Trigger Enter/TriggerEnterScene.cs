using Cysharp.Threading.Tasks;
using UnityEngine;

public class TriggerEnterScene : TriggerEnter
{
    [SerializeField] private SceneType _sceneType;

    protected override void OnPlayerEnter()
    {
        switch(_sceneType)
        {
            case SceneType.TutorialScene:
                SceneLoad.LoadTutorialMap();
                break;
            case SceneType.SelectionScene:
                SceneLoad.LoadSelectStage();
                break;
            default:
                break;
        }
    }
}