using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Open Close Object")]
    [SerializeField] private GameObject openObject;
    [SerializeField] private GameObject closeObject;

    [Header("Audio Source")]
    [SerializeField] private AudioSource m_audioSource;

    public void Activated(bool Open)
    {
        if (openObject) openObject.SetActive(Open);
        if (closeObject) closeObject.SetActive(!Open);
        gameObject.SetActive(true);

        if (Open) AutoSaveScene.SaveObjectState(name);
    }
    public void ActivatedWithSound(bool Open)
    {
        if (openObject) openObject.SetActive(Open);
        if (closeObject) closeObject.SetActive(!Open);
        gameObject.SetActive(true);

        if (Open)
        {
            AutoSaveScene.SaveObjectState(name);
            if (AudioManager.Instance != null) AudioManager.Instance.PlayDoorOpen(m_audioSource);
        }
        else
        {
            AutoSaveScene.SaveObjectState(name + " | Close");
            if (AudioManager.Instance != null) AudioManager.Instance.PlayDoorOpen(m_audioSource);
        }
    }
}