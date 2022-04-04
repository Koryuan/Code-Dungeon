using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InputAbleManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text actionList_Text;

    [Header("Action List")]
    [SerializeField] private List<CodeAction> inputAbleAction = new List<CodeAction>();

    [Header("IDE")]
    [SerializeField] private IDECompiler compiler;

    public event Action<string> OnWrongCodeLine;

    private void Awake()
    {
        foreach(CodeAction action in inputAbleAction) actionList_Text.text += action.assignedName + "\n";
        compiler.OnCompileFunction += CompileFunction;
    }

    private void CompileFunction(string code, int line)
    {
        int stopIndex = code.IndexOf('(');
        if (stopIndex <= 0)
        {
            OnWrongCodeLine?.Invoke($"There is no bracket for parameter on line {line}");
            return;
        }

        string functionName = code.Substring(0, stopIndex);
        bool isAFunction = DoAction(functionName);
        if (!isAFunction) OnWrongCodeLine?.Invoke($"There is no such function with '{functionName}' name");
    }

    private bool DoAction(string FunctionName)
    {
        foreach (CodeAction action in inputAbleAction)
        {
            if (action.assignedName == FunctionName)
            {
                action.actionToDo.Invoke();
                return true;
            }
        }
        return false;
    }
}


[Serializable] public class CodeAction
{
    public string assignedName;
    public UnityEvent actionToDo;
}
