using UnityEngine;

public class CompilePrintStatic : CodeCompiler
{
    private string[] m_printedText;

    public override string[] PrintCompile(string[] InputFieldData) => CheckText(m_printedText);
}