using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompileTwoIntArithmatic : CodeCompiler
{
    [Header("New Properties")]
    [SerializeField] private int m_firstVariable;
    [SerializeField] private int m_secondVariable;

    public override string[] PrintCompile(string[] InputFieldData)
    {
        if (InputFieldData.Length <= 0) return CheckText(new[] { "ERROR" }); 
        string Result = GetArithmaticValueINT(m_firstVariable, m_secondVariable, InputFieldData[0]);
        string[] CheckResult = {Result};
        return CheckText(CheckResult);
    }
}