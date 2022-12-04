using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

[Serializable] public class SaveLoadMenuUI
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Title")]
    [SerializeField] private TMP_Text titleText;

    [Header("Safe File")]
    [SerializeField] private SaveFileButton[] saveFileButtons;

    public bool IsOpen => panel.activeSelf;

    #region Initialization
    public void Initialization(string ObjectName)
    {
        CheckReferences(ObjectName);

        int count = 1;
        foreach(SaveFileButton Button in saveFileButtons)
        {
            string fileName = $"Save Data - {count}";
            Button.Initialize(SaveLoadSystem.Instance.LoadFile(fileName), fileName);
            Button.SetHighlight(false);
            count++;
        }
    }
    private void CheckReferences(string ObjectName)
    {
        if (!panel) Debug.LogError($"{ObjectName} has no Menu Panel references");
        if (!titleText) Debug.LogError($"{ObjectName} has no title text references");
        if (saveFileButtons.Length == 0) Debug.LogError($"{ObjectName} is lacking button references");
    }
    #endregion

    async public void Panel(bool IsOpen)
    {
        panel.SetActive(IsOpen);
        await UniTask.Delay(10);
        if (IsOpen) saveFileButtons[0].Select();
    }
    public void UpdateUIState(bool IsSave)
    {
        titleText.text = IsSave ? "Save" : "Load";
        foreach (SaveFileButton Button in saveFileButtons) Button.UpdateState(IsSave);
    }
}