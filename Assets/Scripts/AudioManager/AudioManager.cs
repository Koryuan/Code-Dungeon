using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager m_instance;
    public static AudioManager Instance => m_instance;

    private float sfxVolume 
        => (SaveLoadSystem.Instance?._SaveData != null) ? SaveLoadSystem.Instance._SaveData.sfxVolume : 1;
    private float bgmVolume 
        => (SaveLoadSystem.Instance?._SaveData != null) ? SaveLoadSystem.Instance._SaveData.bgmVolume : 1;

    [System.Serializable]
    private class ObjectAudioClip
    {
        // Door
        public AudioClip Object_DoorOpen;
        public AudioClip Object_DoorClose;
    }
    [SerializeField] private ObjectAudioClip ObjectAudio;

    [System.Serializable]
    private class PlayerAudioClip
    {
        public AudioClip Player_Walk;
    }
    [SerializeField] private PlayerAudioClip PlayerAudio;

    [System.Serializable]
    private class UIAudioClip
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
        Source.volume = sfxVolume;
        Source.Play();
    }
    #endregion

    #region Object
    public void PlayDoorOpen(AudioSource Source)
    {
        Source.volume = sfxVolume;
        Source.PlayOneShot(ObjectAudio.Object_DoorOpen);
    }
    public void PlayDoorClose(AudioSource Source)
    {
        Source.volume = sfxVolume;
        Source.PlayOneShot(ObjectAudio.Object_DoorClose);
    }
    #endregion

    #region UI
    public void PlayUIConfirm()
    {
        if (MainAudioSource.Instance)
        {
            MainAudioSource.Instance.Source.volume = sfxVolume;
            MainAudioSource.Instance.Source.PlayOneShot(UIAudio.UI_Confirm);
        }
    }
    public void PlayUIHover()
    {
        if (MainAudioSource.Instance)
        {
            MainAudioSource.Instance.Source.volume = sfxVolume;
            MainAudioSource.Instance.Source.PlayOneShot(UIAudio.UI_Hover);
        }
    }
    #endregion
}