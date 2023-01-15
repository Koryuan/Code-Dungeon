using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager m_instance;
    public static AudioManager Instance => m_instance;

    private float sfxVolume 
        => (SaveLoadSystem.Instance?._SaveData != null) ? SaveLoadSystem.Instance._MasterData.SFXVolume : 1;
    private float bgmVolume 
        => (SaveLoadSystem.Instance?._SaveData != null) ? SaveLoadSystem.Instance._MasterData.BGMVolume : 1;

    [SerializeField] private AudioSource m_bgmAudioSource;
    [SerializeField] private AudioSource m_uiAudioSource;

    [System.Serializable] private class ObjectAudioClip
    {
        // Door
        public AudioClip Object_DoorOpen;
        public AudioClip Object_DoorClose;
        public AudioClip Object_TreasureChest_Open;
    } [SerializeField] private ObjectAudioClip ObjectAudio;

    [System.Serializable]
    private class PlayerAudioClip
    {
        public AudioClip Player_Walk;
    } [SerializeField] private PlayerAudioClip PlayerAudio;

    [System.Serializable] private class UIAudioClip
    {
        public AudioClip UI_Confirm;
        public AudioClip UI_Hover;
    } [SerializeField] private UIAudioClip m_uiAudio;

    [System.Serializable] private class BGMAudioClip
    {
        public AudioClip BGM_MainMenu;
        public AudioClip BGM_InGame;
    } [SerializeField] private BGMAudioClip m_bgmAudioClip;

    private void Awake()
    {
        if (m_instance != null && m_instance != this) Destroy(gameObject);
        else
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdateSFXVolume() => m_uiAudioSource.volume = sfxVolume;
    public void UpdateBGMVolume() => m_bgmAudioSource.volume = bgmVolume;

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
    public void PlayTreasureChestOpen(AudioSource Source)
    {
        Source.volume = sfxVolume;
        Source.PlayOneShot(ObjectAudio.Object_TreasureChest_Open);
    }
    #endregion

    #region UI
    public void PlayUIConfirm()
    {
        if (!m_uiAudioSource) return;

        m_uiAudioSource.volume = sfxVolume;
        m_uiAudioSource.PlayOneShot(m_uiAudio.UI_Confirm);
    }
    public void PlayUIHover()
    {
        if (!m_uiAudioSource) return;
        
        m_uiAudioSource.volume = sfxVolume;
        m_uiAudioSource.PlayOneShot(m_uiAudio.UI_Hover);
    }
    #endregion

    #region BGM
    public void PlayBGMMainMenu()
    {
        if (!m_bgmAudioSource) return;

        m_bgmAudioSource.loop = true;
        m_bgmAudioSource.volume = bgmVolume;

        if (m_bgmAudioSource.isPlaying && m_bgmAudioSource.clip == m_bgmAudioClip.BGM_MainMenu) return;

        m_bgmAudioSource.clip = m_bgmAudioClip.BGM_MainMenu;
        m_bgmAudioSource.Play();
    }
    public void PlayBGMInGame()
    {
        if (!m_bgmAudioSource) return;

        m_bgmAudioSource.loop = true;
        m_bgmAudioSource.volume = bgmVolume;

        if (m_bgmAudioSource.isPlaying && m_bgmAudioSource.clip == m_bgmAudioClip.BGM_InGame) return;

        m_bgmAudioSource.clip = m_bgmAudioClip.BGM_InGame;
        m_bgmAudioSource.Play();
    }
    #endregion
}