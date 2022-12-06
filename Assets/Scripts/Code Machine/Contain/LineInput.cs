using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineInput : CodeMachineContain
{
    [System.Serializable] public struct InputField
    {
        public TMP_InputField Input;
        public TMP_Text Text;
        public string Target;

        public bool ReachTarget() => Input.text.Replace(StringList.HTML_Underline_Front, string.Empty) == Target;
    }

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

    private void UpdateColor(InputField Input)
    {
        if (Input.ReachTarget()) Input.Text.color = new Color(0, 255, 87);
        else Input.Text.color = Color.black;
    }

    protected override void CheckReferences()
    {
        base.CheckReferences();
        if (startText == string.Empty) Debug.LogError($"{name} in {transform.parent.name}, has no starting text for code line");
        if (m_inputField.Length == 0) Debug.LogError($"{name} in {transform.parent.name}, has no input field for code line");
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