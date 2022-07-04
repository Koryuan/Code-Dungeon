using UnityEngine;

public class TutorialStart : MonoBehaviour
{
    [SerializeField] private DialogSetting dialog;

    private void OnEnable()
    {
        GameManager.Instance.OpenDialogBox(dialog);
    }
}