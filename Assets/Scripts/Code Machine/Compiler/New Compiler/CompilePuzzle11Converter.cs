public class CompilePuzzle11Converter : CodeCompiler
{
    private bool m_blueFairy = false;
    private bool m_redFairy = false;
    private bool m_yellowFairy = false;

    public void UpdateBlueFairy(bool Available) => m_blueFairy = Available;
    public void UpdateRedFairy(bool Available) => m_redFairy = Available;
    public void UpdateYellowFairy(bool Available) => m_yellowFairy = Available;

    public override string[] PrintCompile(string[] InputFieldData)
    {
        if (m_blueFairy) return CheckText(new[] {"Blue Essence" });
        if (m_redFairy) return CheckText(new[] { "Red Essence" });
        if (m_yellowFairy) return CheckText(new[] { "Yellow Essence" });
        return CheckText(new[] { "No Output" });
    }
}