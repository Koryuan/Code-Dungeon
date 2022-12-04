using UnityEngine;

public class MainAudioSource : MonoBehaviour
{
    private static MainAudioSource m_instance { get; set;}
    public static MainAudioSource Instance => m_instance;

    [SerializeField] private AudioSource m_source;

    public AudioSource Source
    {
        get
        {
            if (!m_source) m_source = gameObject.AddComponent<AudioSource>();
            return m_source;
        }
    }

    private void Awake()
    {
        m_instance = this;
    }
}