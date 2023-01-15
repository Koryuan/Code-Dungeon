using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KeypadManager : MonoBehaviour
{
    private int currentNumber = 0;
    private string currentText = string.Empty;
    private string currentTextSave = string.Empty;

    private AutoSaveKeypad m_autoSave;

    [Header("General")]
    [SerializeField] private int m_maxText;

    [System.Serializable] private class KeyPadGameEvent
    {
        [SerializeField] private string name;
        [SerializeField] private string[] m_targetText;
        [SerializeField] private int m_occurenceNumber;
        [SerializeField] private GameEvent[] m_eventList;

        public int OccurenceNumber => m_occurenceNumber;
        public GameEvent[] EventList => m_eventList;

        public void Occur()
        {
            if (m_occurenceNumber != -1) m_occurenceNumber--;
        }
        public bool IsTargetText(string InputedText)
        {
            foreach (string text in m_targetText) if (text == InputedText) return true;
            return false;
        }
    }

    [Header("List")]
    [SerializeField] private List<Keypad> m_keyList = new List<Keypad>();
    [SerializeField] private KeyPadGameEvent[] m_eventList;

    private void Awake()
    {
        if (m_maxText <= 0) Debug.LogError($"{name}, target cann't be write");
        foreach(Keypad key in m_keyList) key.OnInteracted += AppendText;
        
        // Loaded Data
        m_autoSave = GetComponent<AutoSaveKeypad>();
        if (!m_autoSave) return;

        m_autoSave.OnDataLoaded += LoadData;
        m_autoSave.LoadData();
    }

    #region Loading
    private void LoadData(SaveDataAuto LoadedData)
    {
        if (LoadedData.New) return;

        if (LoadedData is KeypadSaveData OldData) LoadKeyPad(OldData.OccuredText.ToArray(), OldData.LastText);
    }
    public void LoadKeyPad(string[] TargetList, string LastTarget)
    {
        // Update occurence number
        if (TargetList == null) return;
        foreach (string target in TargetList) SearchTargetText(target)?.Occur();

        // Update current text
        if (LastTarget == string.Empty) return;
        string[] splitedText = LastTarget.Split('|');
        foreach (string text in splitedText)
        {
            Keypad key = SearchTargetKey(text);
            if (!key) continue;

            key.UpdateLook(true);
            AppendText(text);
        }
    }
    #endregion

    async private void AppendText(string AppendedText)
    {
        if (currentNumber >= m_maxText)
        {
            currentText = string.Empty;
            currentTextSave = string.Empty;
            currentNumber = 0;
            CloseAllKey();
        }

        currentText += AppendedText;
        currentNumber++;

        // Auto Save
        currentTextSave += AppendedText + '|';
        AutoSaveScene.SaveObjectState(name,currentTextSave);
        if (m_autoSave) m_autoSave.UpdateCurrentText(currentTextSave);

        if (currentNumber == m_maxText)
        {
            KeyPadGameEvent TargetGameEvent = SearchTargetText(currentText);
            if (TargetGameEvent != null)
            {
                TargetGameEvent?.Occur();
                if (m_autoSave) m_autoSave.UpdateOccuredText(currentText);
                if (GameManager.Instance) await GameManager.Instance.StartEvent(TargetGameEvent.EventList);
            }
            ActiveAllKeyInteraction();
        }
    }

    private KeyPadGameEvent SearchTargetText(string TargetText)
    {
        foreach (KeyPadGameEvent keyEvent in m_eventList)
        {
            if (keyEvent.IsTargetText(TargetText) && keyEvent.OccurenceNumber != 0) return keyEvent;
        } return null;
    }
    private Keypad SearchTargetKey(string TargetText)
    {
        foreach (Keypad key in m_keyList)
        {
            if (key.Text == TargetText) return key;
        }
        return null;
    }

    private void CloseAllKey()
    {
        foreach (Keypad key in m_keyList) key.UpdateLook(false);
    }
    private void ActiveAllKeyInteraction()
    {
        foreach (Keypad key in m_keyList) key.UpdateInteraction(true);
    }
}