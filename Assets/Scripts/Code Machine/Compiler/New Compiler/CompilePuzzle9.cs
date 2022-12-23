public class CompilePuzzle9 : CodeCompiler
{
    public override string[] ScanCompile(string[] InputFieldData, string[] ScanData)
    {
        if (ScanData.Length <= 0) return CheckText(new[] { "ERROR" });

        bool parseSuccesfully = int.TryParse(ScanData[0], out int inputValue);
        if (!parseSuccesfully) return CheckText(new[] { "Input is not", "Interger" });

        // Actual Code
        if (inputValue > 15) inputValue %= 10;
        inputValue -= 10;

        if (inputValue >= 1) return CheckText(new[] { "Open Treasure Chest" });
        return CheckText(new[] { "Nothing Happen" });
    }
}