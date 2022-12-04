using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Open Close Object")]
    [SerializeField] private GameObject openObject;
    [SerializeField] private GameObject closeSprite;

    [Header("Audio Source")]
    [SerializeField] private AudioSource m_audioSource;

    public void Activated(bool Open)
    {
        openObject.SetActive(Open);
        closeSprite.SetActive(!Open);
        gameObject.SetActive(true);

        if (Open) AutoSaveScene.SaveObjectState(name);
    }
    public void ActivatedWithSound(bool Open)
    {
        openObject.SetActive(Open);
        closeSprite.SetActive(!Open);
        gameObject.SetActive(true);

        if (Open)
        {
            AutoSaveScene.SaveObjectState(name);
            if (AudioManager.Instance != null) AudioManager.Instance.PlayDoorOpen(m_audioSource);
        }
    }
}