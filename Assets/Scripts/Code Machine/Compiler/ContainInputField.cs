using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContainInputField : MonoBehaviour
{
    [SerializeField] private InputField[] m_inputField;

    public ContainInputSaveData SaveData { get; private set; } = new ContainInputSaveData();
    public Action<ContainInputSaveData> OnUpdate;

    public void Initailize(int Index)
    {
        SaveData.Index = Index;
        for (int i = 0;i < m_inputField.Length;i++)
        {
            ContainInputSaveData.InputData data = new ContainInputSaveData.InputData();
            data.Index = i;
            data.Text = m_inputField[i].Text.text;

            SaveData.InputList.Add(data);
            m_inputField[i].Input.onEndEdit.AddListener((Text) => OnUpdateText(Text,data.Index));
            //m_inputField[i].Input.onEndEdit.AddListener((Text) => UpdateColor(m_inputField[i]));
            //m_inputField[i].Input.onEndEdit.AddListener((Text) => OnUpdate?.Invoke(SaveData));
        }
    }
    public void LoadData(ContainInputSaveData LoadedData)
    {
        if (LoadedData.Index != SaveData.Index) return;

        SaveData = LoadedData;
        for(int i = 0;i < SaveData.InputList.Count; i++)
        {
            m_inputField[i].Input.text = SaveData.InputList[i].Text;
            UpdateColor(m_inputField[i]);
        }
    }
    private void UpdateColor(InputField Input)
    {
        if (Input.ReachTarget()) Input.Text.color = new Color(0, 255, 87);
        else Input.Text.color = Color.black;
    }
    public string[] GetInputFieldText()
    {
        string[] returnedString = new string[m_inputField.Length];
        for (int i = 0; i < m_inputField.Length; i++)
        {
            returnedString[i] = m_inputField[i].Input.text.Replace(StringList.HTML_Underline_Front, string.Empty);
        }
        return returnedString;
    }
    private void OnUpdateText(string Text, int Index)
    {
        if (SaveData.InputList.Count > Index) SaveData.InputList[Index].Text = Text;
        UpdateColor(m_inputField[Index]);
        OnUpdate?.Invoke(SaveData);
    }
    private void OnDestroy()
    {
        foreach (InputField input in m_inputField) input.Input.onEndEdit.RemoveAllListeners();
    }
}

[Serializable] public class ContainInputSaveData
{
    [Serializable] public class InputData
    {
        public int Index;
        public string Text;
    }

    public int Index = -1;
    public List<InputData> InputList = new List<InputData>();
}

[Serializable] public struct InputField
{
    public TMP_InputField Input;
    public TMP_Text Text;
    public string Target;

    public bool ReachTarget() => Input.text.Replace(StringList.HTML_Underline_Front, string.Empty) == Target;
}