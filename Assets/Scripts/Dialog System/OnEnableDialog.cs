using Cysharp.Threading.Tasks;
using UnityEngine;

public class OnEnableDialog : MonoBehaviour
{
    [SerializeField] private DialogSetting dialog;

    private async void OnEnable()
    {
        await UniTask.WaitUntil(()=> GameManager.Instance.CurrentState == GameState.Game_Player_State);

        GameManager.Instance.OpenDialogBox(dialog);
        AutoSaveScene.SaveObjectState(gameObject.name);
        gameObject.SetActive(false);
    }
}