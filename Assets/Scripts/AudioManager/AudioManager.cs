using JetBrains.Annotations;
using UnityEditor.SearchService;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager m_instance;
    public static AudioManager Instance => m_instance;

    [System.Serializable] private class ObjectAudioClip
    {
        // Door
        public AudioClip Object_DoorOpen;
        public AudioClip Object_DoorClose;
    }
    [SerializeField] private ObjectAudioClip ObjectAudio;

    [System.Serializable] private class PlayerAudioClip
    {
        public AudioClip Player_Walk;
    }
    [SerializeField] private PlayerAudioClip PlayerAudio;

    [System.Serializable] private class UIAudioClip
    {
        public AudioClip UI_Confirm;
        public AudioClip UI_Hover;
    }
    [SerializeField] private UIAudioClip UIAudio;

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(m_instance.gameObject);
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #region Player
    public void PlayPlayerWalk(AudioSource Source)
    {
        Source.loop = true;
        Source.clip = PlayerAudio.Player_Walk;
        Source.Play();
    }
    #endregion

    #region Object
    public void PlayDoorOpen(AudioSource Source) => Source.PlayOneShot(ObjectAudio.Object_DoorOpen);
    #endregion

    #region UI
    public void PlayUIConfirm()
    {
        if (MainAudioSource.Instance) MainAudioSource.Instance.Source.PlayOneShot(UIAudio.UI_Confirm);
    }
    public void PlayUIHover()
    {
        if (MainAudioSource.Instance) MainAudioSource.Instance.Source.PlayOneShot(UIAudio.UI_Hover);
    }
    #endregion
}