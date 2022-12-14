public class CompilePuzzle5Arith : CodeCompiler
{
    public override string[] ScanCompile(string[] InputFieldData, string[] ScanData)
    {
        bool parseSuccesfully = int.TryParse(ScanData[0],out int inputValue);
        if (!parseSuccesfully) return CheckText(new[] {"Input is not","Interger"});
        int Result = 10 + 5 * inputValue;
        return CheckText(new[] { Result.ToString()});
    }
}