using TMPro;
using UnityEngine;

public class PopUpMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private string targetText;

    private bool m_correct;
    public bool Correct => m_correct;

    public void OpenMessage(string Text)
    {
        gameObject.SetActive(true);
        if (Text == targetText)
        {
            Text = StringList.ColorString(Text, StringList.Color_LightGreen);
            m_correct = true;
        }
        else Text = StringList.ColorString(Text,StringList.Color_Red);
        m_text.text = Text;
    }
}