using UnityEngine;

public class TestInteractionObject : InteractableTarget
{
    [SerializeField] private DialogSetting dialog;

    public override void OnInteract()
    {
        if (!dialog) Debug.LogError("There is no dialog");
        else DialogBox.instance.OpenDialog(dialog);
    }

    public override void OnUnInteract()
    {
    }
}