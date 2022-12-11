using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Help/Help Setting")]
public class HelpSettings : ScriptableObject
{
    [Serializable] public struct Setting
    {
        [SerializeField] private string m_text;
        [SerializeField] private Sprite m_image;

        public (string Text, Sprite Image) Data => (m_text, m_image);
    }

    [SerializeField] private string m_name;
    [SerializeField] private Setting[] m_Settings;

    public string Name => m_name;
    public Setting[] Settings => m_Settings;
}