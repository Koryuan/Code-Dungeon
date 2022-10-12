using UnityEngine;

public class OnEnableDialog : MonoBehaviour
{
    [SerializeField] private DialogSetting dialog;

    private void OnEnable()
    {
        GameManager.Instance.OpenDialogBox(dialog);
        AutoSaveScene.SaveObjectState(gameObject.name);
        gameObject.SetActive(false);
    }
}