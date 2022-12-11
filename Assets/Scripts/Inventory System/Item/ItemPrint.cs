using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item/Comment Unlocker")]
public class ItemPrint: Item
{
    [Header("Special Attribute")]
    [SerializeField] private float range;
    [SerializeField] private string searchedText;

    [Header("At Failed")]
    [SerializeField] private DialogSetting atFailedDialog;

    public float Range => range;

    public override bool Use()
    {
        PlayerController player = GameManager.Instance.Player;
        Collider2D[] collidedList = Physics2D.OverlapCircleAll(player.transform.position, range);
        foreach (Collider2D collid in collidedList){
            var codeMachine = collid.GetComponent<CodeMachine>();
            if (codeMachine && codeMachine.UnlockText(searchedText))
            {
                AutoSaveScene.SaveObjectState(codeMachine.name);
                return true;
            }
        }
        GameManager.Instance.OpenDialogBox(atFailedDialog);
        return false;
    }
}