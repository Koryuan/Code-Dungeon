using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KeypadManager : MonoBehaviour
{
    private int currentNumber = 0;
    private string currentText = string.Empty;
    private string currentTextSave = string.Empty;

    [Header("General")]
    [SerializeField] private int m_maxText;

    [System.Serializable] private class KeyPadGameEvent
    {
        [SerializeField] private string m_targetText;
        [SerializeField] private int m_occurenceNumber;
        [SerializeField] private GameEvent[] m_eventList;

        public string TargetText => m_targetText;
        public int OccurenceNumber => m_occurenceNumber;
        public GameEvent[] EventList => m_eventList;

        public void Occur() => m_occurenceNumber--;
    }

    [Header("List")]
    [SerializeField] private List<Keypad> m_keyList = new List<Keypad>();
    [SerializeField] private KeyPadGameEvent[] m_eventList;

    private void Awake()
    {
        if (m_maxText <= 0) Debug.LogError($"{name}, target cann't be write");
        foreach (KeyPadGameEvent gameEvent in m_eventList)
        {
            if (gameEvent.TargetText.Length > m_maxText || gameEvent.TargetText.Length < m_maxText)
                Debug.LogError($"\"{gameEvent.TargetText}\" target text in {name} lenght didn't match");
        }

        foreach(Keypad key in m_keyList) key.OnInteracted += AppendText;
    }

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
        currentTextSave += AppendedText + '|';
        AutoSaveScene.SaveObjectState(name,currentTextSave);
        currentNumber++;

        if (currentNumber == m_maxText)
        {
            KeyPadGameEvent TargetGameEvent = SearchTargetText(currentText);
            if (TargetGameEvent != null)
            {
                TargetGameEvent?.Occur();
                if (GameManager.Instance) await GameManager.Instance.StartEvent(TargetGameEvent.EventList);
            }
            ActiveAllKeyInteraction();
        }
    }

    private KeyPadGameEvent SearchTargetText(string TargetText)
    {
        foreach (KeyPadGameEvent keyEvent in m_eventList)
        {
            if (keyEvent.TargetText == TargetText && keyEvent.OccurenceNumber > 0) return keyEvent;
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
    public void LoadKeyPad(string[] TargetList, string LastTarget)
    {
        if (TargetList != null) 
            foreach(string target in TargetList) SearchTargetText(target)?.Occur();
        if (LastTarget != string.Empty)
        {
            string[] splitedText = LastTarget.Split('|');
            foreach (string text in splitedText) SearchTargetKey(text)?.UpdateLook(true);
        }
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