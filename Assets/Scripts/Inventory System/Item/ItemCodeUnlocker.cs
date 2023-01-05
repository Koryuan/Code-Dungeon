using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item/Code Unlocker")]
public class ItemCodeUnlocker : Item
{
    [Header("Special Attribute")]
    [SerializeField] protected float m_range;
    [SerializeField] protected string m_searchedText;
    [SerializeField] protected string m_changedText;
    [SerializeField] protected bool m_afterChangeFixed = false;

    [Header("Infinite Loop")]
    [SerializeField] protected bool m_infiniteLoop = false;

    [Header("At Failed")]
    [SerializeField] protected DialogSetting m_atFailedDialog;

    public override bool Use()
    {
        PlayerController player = GameManager.Instance.Player;
        Collider2D[] collidedList = Physics2D.OverlapCircleAll(player.transform.position, m_range);
        foreach (Collider2D collid in collidedList)
        {
            var codeMachine = collid.GetComponent<CodeMachineMK2>();
            if (codeMachine && codeMachine.UnlockLine(m_searchedText, m_changedText, m_afterChangeFixed, m_infiniteLoop)) return true;
        }
        GameManager.Instance.OpenDialogBox(m_atFailedDialog);
        return false;
    }
}