using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IDECompiler : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField code_InputField;
    [SerializeField] private Button compile_Button;

    public event Action<string, int> OnCompileFunction;

    private void Awake()
    {
        compile_Button.onClick.AddListener(CompileFullText);
    }

    private void CompileFullText()
    {
        TMP_Text text = code_InputField.textComponent;
        int textInLine = 1;
        foreach (TMP_LineInfo line in text.GetTextInfo(text.text).lineInfo)
        {
            string code = text.text.Substring(line.firstCharacterIndex, line.characterCount);
            if (!string.IsNullOrWhiteSpace(code))
            {
                string[] numberOfFunction = code.Split(';');

                foreach (string function in numberOfFunction)
                {
                    if (string.IsNullOrWhiteSpace(function) || function.Length == 1) continue;
                    OnCompileFunction(function, textInLine);
                }
            }
            textInLine++;
        }
    }
}