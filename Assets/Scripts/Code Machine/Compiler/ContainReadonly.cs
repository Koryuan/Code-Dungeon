using System;
using TMPro;
using UnityEngine;

public class ContainReadonly : MonoBehaviour
{
    [SerializeField] private TMP_Text m_textBox;
    [SerializeField] private bool m_isError = false;

    public ContainReadonlySaveData SaveData { get; private set; } = new ContainReadonlySaveData();
    public string Text => m_textBox.text;
    public bool Error => m_isError;

    public event Action<ContainReadonlySaveData> OnUpdate;

    public void Initialize(int Index)
    {
        SaveData.Index = Index;
        SaveData.Error = m_isError;
        SaveData.Text = m_textBox.text;
    }
    public void LoadData(ContainReadonlySaveData LoadedData)
    {
        if (LoadedData.Index != SaveData.Index) return;
        
        SaveData = LoadedData;
        m_isError = SaveData.Error;
        m_textBox.text = SaveData.Text;
    }
    public void UpdateText(string NewText)
    {
        SaveData.Text = m_textBox.text = NewText;
        OnUpdate?.Invoke(SaveData);
    }
    public void Fixed(bool IsFixed)
    {
        SaveData.Error = m_isError = !IsFixed;
        OnUpdate?.Invoke(SaveData);
    }
}

[Serializable] public class ContainReadonlySaveData
{
    public int Index = -1;
    public string Text = string.Empty;
    public bool Error = false;
}