using System.Collections.Generic;

public class CompilePuzzle22 : CodeCompiler
{
    public override string[] PrintCompile(string[] InputFieldData)
    {
        if (InputFieldData.Length < 0) return CheckText(new[] { "ERROR" });

        bool parseSuccesfully = int.TryParse(InputFieldData[0], out int inputValue);
        if (!parseSuccesfully) return CheckText(new[] { "It was not", "Interger" });

        if (inputValue <= 0) return CheckText(new[] { "INFINITE LOOP" });

        int count = 0; List<string> OutputList = new List<string>();
        int x = 5;
        while(count < 5)
        {
            x = (x * 3) % 4;
            count += inputValue;
            OutputList.Add(x.ToString());
        }
        if (x == 1) OutputList.Add("Remove Monster");

        return CheckText(OutputList.ToArray());
    }
}