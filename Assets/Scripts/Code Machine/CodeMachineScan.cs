using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeMachineScan : MonoBehaviour
{
    [SerializeField] private TMP_InputField[] m_inputFieldList;
    [SerializeField] private Button m_button;

    public event Action<string[]> OnInputButtonClicked;

    public void Initialize()
    {
        if (m_button) m_button.onClick.AddListener(ButtonClick);
    }
    private void ButtonClick() => OnInputButtonClicked?.Invoke(CreateStringList());
    private string[] CreateStringList()
    {
        List<string> ReturnedString = new List<string>();
        foreach (TMP_InputField input in m_inputFieldList)
        {
            ReturnedString.Add(input.text);
            input.text = string.Empty;
        }
        return ReturnedString.ToArray();
    }
    private void OnDestroy()
    {
        if (m_button) m_button.onClick.RemoveAllListeners();
    }
}