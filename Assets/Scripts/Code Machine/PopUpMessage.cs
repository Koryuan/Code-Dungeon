using TMPro;
using UnityEngine;

public class PopUpMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;

    public void OpenMessage(string Text)
    {
        gameObject.SetActive(true);
        m_text.text = Text;
    }
}