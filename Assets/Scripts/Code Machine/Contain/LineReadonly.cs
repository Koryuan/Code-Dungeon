using TMPro;
using UnityEngine;

public class LineReadonly: CodeMachineContain
{
    [SerializeField] private TMP_Text codeLine;

    public override void Initialize()
    {
        base.Initialize();
        BaseText = codeLine.text;
        UpdateText(codeLine.text);
    }

    protected override void CheckReferences()
    {
        base.CheckReferences();
        if (!codeLine) Debug.LogError($"{name} in {transform.parent.name}, has no text for code line");
    }

    public void UpdateText(string NewLine)
    {
        BaseText = NewLine;
        if (NewLine.Contains(StringList.CommentCode))
        {
            int commentIndex = NewLine.IndexOf("//");
            NewLine = NewLine.Substring(0,commentIndex) + StringList.ColorString(NewLine.Substring(commentIndex), StringList.DarkGreenHex);
        }
        codeLine.text = NewLine;
    }
}