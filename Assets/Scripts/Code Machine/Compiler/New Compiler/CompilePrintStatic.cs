using UnityEngine;

public class CompilePrintStatic : CodeCompiler
{
    [Header("Text to print")]
    [SerializeField] private string[] m_printedText;

    public override string[] PrintCompile(string[] InputFieldData) => CheckText(m_printedText);
}