using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemList : MonoBehaviour
{
    private static ItemList m_instance;
    public static ItemList Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("GameInstance").AddComponent<ItemList>();
                DontDestroyOnLoad(m_instance.gameObject);
            }
            return m_instance;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene newScene, LoadSceneMode mode)
    {
        if (newScene.name == "Main Menu Scene") Destroy(gameObject);
    }
}

[System.Serializable] public class Item
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
}