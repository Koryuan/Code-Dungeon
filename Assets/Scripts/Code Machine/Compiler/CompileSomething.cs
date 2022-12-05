using System.Collections.Generic;
using UnityEngine;

public class CompileSomething : MonoBehaviour
{
    private List<string> codeList = new List<string>();

    private void CompileEachCode()
    {
        foreach (string code in codeList)
        {
            bool error = CompileOneLine(code);
            if (error) break;
        }
    }
    private bool CompileOneLine(string Code)
    {
        int count = 0;
        while (Code.Length > 0)
        {
            if (Code.Contains(StringList.Code_Print_Start))
            {
                string newCode = Code.Replace(StringList.Code_Print_Start, string.Empty);
                if (newCode.Contains(StringList.Code_Print_End))
                {
                    int index = newCode.IndexOf(StringList.Code_Print_End);
                    string tmpCode = newCode.Substring(0, index);
                    PrintMessage(tmpCode);
                    newCode = newCode.Replace(tmpCode, string.Empty);
                    newCode = newCode.Replace(StringList.Code_Print_End, string.Empty);
                    Debug.Log("This is runned");
                }
                else
                {
                    PrintMessage("Error");
                    return true;
                }
                Code = newCode;
            }
            #region Infinite LOOP Breaker
            count++;
            if (count > 100)
            {
                Debug.Log($"{count}, {Code}");
                PrintMessage("Infinite Loop");
                return true;
            }
            #endregion
        }
        return false;
    }

    private void PrintMessage(string Text)
    {
        Debug.Log(Text);
    }
}