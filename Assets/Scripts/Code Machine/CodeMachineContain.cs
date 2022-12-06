using TMPro;
using UnityEngine;

public abstract class CodeMachineContain : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] protected TMP_Text m_codeNumber;
    public string BaseText { get; protected set; }

    public virtual void Initialize()
    {
        CheckReferences();
    }

    protected virtual void CheckReferences()
    {
        if (!m_codeNumber) Debug.LogError($"{name} in {transform.parent.name}, has no number in code");
    }
    public virtual string[] GetInputFieldText()
    {
        throw new System.NotImplementedException();
    }
}