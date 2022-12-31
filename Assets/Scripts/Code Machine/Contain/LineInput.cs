using System;
using TMPro;
using UnityEngine;

public class LineInput : CodeMachineContain
{
    [SerializeField] private string parentName = string.Empty;
    [SerializeField] private string startText = string.Empty;
    [SerializeField] private InputField[] m_inputField;

    public override void Initialize()
    {
        base.Initialize();
        BaseText = startText;
        foreach(InputField input in m_inputField)
        {
            input.Input.onEndEdit.AddListener((Text) => UpdateColor(input));
        }
    }
    protected override void CheckReferences()
    {
        base.CheckReferences();
        if (startText == string.Empty) Debug.LogError($"{name} in {transform.parent.name}, has no starting text for code line");
        if (m_inputField.Length == 0) Debug.LogError($"{name} in {transform.parent.name}, has no input field for code line");
    }

    public void UpdateText(InputFieldLine NewText)
    {
        Debug.Log($"{m_codeNumber.text} and {NewText.Number}");
        if (m_codeNumber.text != NewText.Number) return;
        Debug.Log("Update Text");
        m_inputField[NewText.IndexInArray].Input.text = NewText.Text;
        UpdateColor(m_inputField[NewText.IndexInArray]);
    }
    private void UpdateColor(InputField Input)
    {
        if (Input.ReachTarget()) Input.Text.color = new Color(0, 255, 87);
        else Input.Text.color = Color.black;
    }

    public override string[] GetInputFieldText()
    {
        string[] returnedString = new string[m_inputField.Length];
        for(int i = 0; i < m_inputField.Length;i++)
        {
            returnedString[i] = m_inputField[i].Input.text.Replace(StringList.HTML_Underline_Front,string.Empty);
        }
        return returnedString;
    }
}

[Serializable] public struct InputField
{
    public TMP_InputField Input;
    public TMP_Text Text;
    public string Target;

    public bool ReachTarget() => Input.text.Replace(StringList.HTML_Underline_Front, string.Empty) == Target;
}

[Serializable] public class InputFieldLine
{
    public string Number;
    public int IndexInArray;
    public string Text;

    public InputFieldLine(string Number = "", int IndexInArray = -1, string Text = "")
    {
        this.Number = Number;
        this.IndexInArray = IndexInArray;
        this.Text = Text;
    }

}