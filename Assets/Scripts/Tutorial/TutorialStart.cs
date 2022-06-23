using UnityEngine;

public class TutorialStart : MonoBehaviour
{
    [SerializeField] private DialogBox dialogSystem;
    [SerializeField] private DialogSetting dialog;

    private void OnEnable()
    {
        dialogSystem.OpenDialog(dialog);
    }
}