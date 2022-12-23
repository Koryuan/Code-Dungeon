public class CompileSwitchCase : CodeCompiler
{
    [System.Serializable] public struct SwitchData
    {
        [UnityEngine.SerializeField] private string name;
        [UnityEngine.SerializeField] private string[] m_targetText;
        [UnityEngine.SerializeField] private string[] m_returnText;

        public string[] GetReturn(string[] InputText)
        {
            if (InputText.Length != m_targetText.Length) return null;
            for (int i = 0;i < m_targetText.Length;i++)
                if (InputText[i] != m_targetText[i]) return null;
            return m_returnText;
        }
    }
    [UnityEngine.Header("Input to Check")]
    [UnityEngine.SerializeField] private SwitchData[] m_inputList;
    [UnityEngine.SerializeField] private string[] m_default;

    public override string[] ScanCompile(string[] InputFieldData, string[] ScanData)
    {
        if (ScanData.Length <= 0) return CheckText(new[] { "ERROR" });
        foreach(SwitchData data in m_inputList)
        {
            string[] result = data.GetReturn(ScanData);
            if (result == null) continue;

            return CheckText(result);
        }
        return CheckText(m_default);
    }
}