using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item/Code Unlocker")]
public class ItemCodeUnlocker : Item
{
    [Header("Special Attribute")]
    [SerializeField] private float m_range;
    [SerializeField] private string m_searchedText;
    [SerializeField] private string m_changedText;
    [SerializeField] private bool m_afterChangeFixed = false;

    [Header("At Failed")]
    [SerializeField] private DialogSetting m_atFailedDialog;

    public override bool Use()
    {
        PlayerController player = GameManager.Instance.Player;
        Collider2D[] collidedList = Physics2D.OverlapCircleAll(player.transform.position, m_range);
        foreach (Collider2D collid in collidedList)
        {
            var codeMachine = collid.GetComponent<CodeMachineMK2>();
            if (codeMachine && codeMachine.UnlockLine(m_searchedText, m_changedText, m_afterChangeFixed)) return true;
        }
        GameManager.Instance.OpenDialogBox(m_atFailedDialog);
        return false;
    }
}