using System.Collections;
using UnityEngine;

public class CompilePrintInput : CodeCompiler
{
    public override string[] PrintCompile(string[] InputFieldData)
    {
        return CheckText(InputFieldData);
    }
    public override string[] ScanCompile(string[] InputFieldData, string[] ScanData)
    {
        return CheckText(ScanData);
    }
}