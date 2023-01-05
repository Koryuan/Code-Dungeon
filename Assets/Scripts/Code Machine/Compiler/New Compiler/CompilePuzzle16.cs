using System.Collections.Generic;

public class CompilePuzzle16 : CodeCompiler
{
    public override string[] ScanCompile(string[] InputFieldData, string[] ScanData)
    {
        if (ScanData.Length <= 0 || InputFieldData.Length <= 0) return CheckText(new[] { "ERROR" });

        bool parseSuccesfully = int.TryParse(ScanData[0], out int inputValue);
        if (!parseSuccesfully) return CheckText(new[] { "Input is not", "Interger" });

        List<string> OutputList = new List<string>();

        // Actual Code
        if (inputValue != 5) OutputList.Add("You Death");
        if (InputFieldData[0] == "False") OutputList.Add("You Death");
        else if (InputFieldData[0] == "True") OutputList.Add("Door Unlocked");
        else OutputList.Add("ERROR, CanOpen Is not Boolean");

        return CheckText(OutputList.ToArray());
    }
}