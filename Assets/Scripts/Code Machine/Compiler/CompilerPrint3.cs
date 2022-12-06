using UnityEngine;

public class CompilerPrint3 : CompileObject
{
    public override string CompileCodeReturnString(string[] CodeList)
    {
        string returnedString = "";
        foreach(string line in CodeList)
        {
            returnedString = line;
        }
        return returnedString;
    }
}