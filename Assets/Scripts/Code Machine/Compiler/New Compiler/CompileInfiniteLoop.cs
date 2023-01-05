using UnityEngine;

public class CompileInfiniteLoop : CodeCompiler
{
    [SerializeField] private string[] OnNotInfiniteLoop;

    public override string[] PrintCompile(string[] InputFieldData)
    {
        if (m_infiniteLoop) return CheckText(new[] { "INFINITE LOOP"});

        if (InputFieldData.Length > 0)
        {
            bool parseSuccesfully = int.TryParse(InputFieldData[0], out int inputValue);
            if (!parseSuccesfully) return CheckText(new[] { "It was not", "Interger" });

            if (inputValue <= 0) return CheckText(new[] { "INFINITE LOOP" });
        }

        return CheckText(OnNotInfiniteLoop);
    }
}