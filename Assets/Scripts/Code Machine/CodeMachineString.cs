using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class CodeMachineString : CodeMachine
{
    private CompileObject m_compiler;

    protected override void Awake()
    {
        base.Awake();
        m_compiler = GetComponent<CompileObject>();
    }

    async protected override UniTask PrintInteraction()
    {
        if (m_compiler)
        {
            string[] codeList = null;
            foreach(CodeMachineContain contain in m_containList)
            {
                if (contain is LineInput)
                {
                    codeList = ConcatArray.AddString(codeList, contain.GetInputFieldText());
                }
                else continue;
            }
            if (codeList.Length > 0) PrintMessage(m_compiler.CompileCodeReturnString(codeList));
        }
        if (printFunction)
        {
            await printFunction.Activate(printMessage.Correct);
            AutoSaveScene.SaveObjectState($"{name} | Print");
            Debug.Log($"{name} | Print");
        }
        else Debug.LogError($"{name}, trying to use function from print without having the class");
    }
}

public static class ConcatArray
{
    public static string[] AddString(string[] a, string[] b)
    {
        if (a == null) return b;
        else if (b == null) return a;
        List<string> c = new List<string>();
        c.AddRange(a); c.AddRange(b);
        string[] NewArray = c.ToArray();
        return NewArray;
    }
}