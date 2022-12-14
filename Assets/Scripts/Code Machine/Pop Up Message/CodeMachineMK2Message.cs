using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeMachineMK2Message : MonoBehaviour
{
    [Header("Single Message")]
    [SerializeField] private PopUpMessageMK2 m_singleMessage;

    [Header("Multiple Message")]
    [SerializeField] private Transform m_container;
    [SerializeField] private PopUpMessageMK2 m_messagePrefab = null;
    [SerializeField] private ScrollbarNew m_scrollbar;

    private List<PopUpMessageMK2> m_popUpList = new List<PopUpMessageMK2>();

    public void PrintMessage(string[] MessagesToPrint)
    {
        DeleteAllMessage();
        CloseOtherMessage();

        m_container.gameObject.SetActive(true);

        foreach (string message in MessagesToPrint)
        {
            var printedMessage = Instantiate(m_messagePrefab, m_container);
            printedMessage.UpdateMessage(message);
            if (m_scrollbar) m_scrollbar.CenterOnItem(printedMessage.ObjectTransform);
            m_popUpList.Add(printedMessage);
        }
    }

    public void PrintOnly1Message(string MessageToPrint)
    {
        CloseOtherMessage();
        m_singleMessage.gameObject.SetActive(true);
        m_singleMessage.UpdateMessage(MessageToPrint);
    }

    private void DeleteAllMessage()
    {
        if (m_popUpList.Count == 0) return;
        for (int i = 0; i < m_popUpList.Count; i++) Destroy(m_popUpList[i].gameObject);
        m_popUpList.Clear();
    }
    private void CloseOtherMessage()
    {
        m_container.gameObject.SetActive(false);
        m_singleMessage.gameObject.SetActive(false);
    }
}