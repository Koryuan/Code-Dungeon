public class CompileOneBool : CodeCompiler
{
    [UnityEngine.Header("True/False Return")]
    [UnityEngine.SerializeField] private string[] OnTrue;
    [UnityEngine.SerializeField] private string[] OnFalse;
    [UnityEngine.SerializeField] private string[] OnDefault;

    public override string[] PrintCompile(string[] InputFieldData)
    {
        if (InputFieldData.Length <= 0) return CheckText(new[] { "ERROR" });
        return CheckText(CheckInput(InputFieldData[0]));
    }
    private string[] CheckInput(string Value) => (Value) switch
    {
        "True" => OnTrue,
        "False" => OnFalse,
        _ => OnDefault
    };
}