using JetBrains.Annotations;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Open Close")]
    [SerializeField] private bool isOpen = false;
    [SerializeField] private GameObject openObject;
    [SerializeField] private GameObject closeObject;

    [Header("Audio Source")]
    [SerializeField] private AudioSource m_audioSource;

    private AutoSaveDoor m_autoSave = null;

    private void Awake()
    {
        m_autoSave = GetComponent<AutoSaveDoor>();
        Activated(isOpen);
        
        if (!m_autoSave) return;

        m_autoSave.OnDataLoaded += LoadData;
        m_autoSave.LoadData(isOpen);
    }
    private void LoadData(SaveDataAuto LoadedData)
    {
        if (LoadedData.New) return;
        //Debug.Log($"{name}, Loaded from save data");
        if (LoadedData is DoorSaveData oldData) Activated(oldData.Open);
    }

    public void Activated(bool Open)
    {
        isOpen = Open;
        if (openObject) openObject.SetActive(Open);
        if (closeObject) closeObject.SetActive(!Open);
    }
    public void ActivatedWithSound(bool Open)
    {
        Activated(Open);
        if (m_autoSave) m_autoSave.UpdateOpen(Open);
        if (Open)
        {
            AutoSaveScene.SaveObjectState(name);
            if (AudioManager.Instance != null) AudioManager.Instance.PlayDoorOpen(m_audioSource);
        }
        else
        {
            AutoSaveScene.SaveObjectState(name + " | Close");
            if (AudioManager.Instance != null) AudioManager.Instance.PlayDoorClose(m_audioSource);
        }
    }

    private void OnDestroy()
    {
        if (m_autoSave) m_autoSave.OnDataLoaded -= LoadData;
    }
}