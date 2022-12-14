using TMPro;
using UnityEngine;

public class PopUpMessageMK2 : MonoBehaviour
{
    [SerializeField] private RectTransform m_transform;
    [SerializeField] private TMP_Text m_text;

    public RectTransform ObjectTransform => m_transform;

    public void UpdateMessage(string Text) => m_text.text = Text;
}