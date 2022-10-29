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

    private List<Item> _itemList;

    private void Awake()
    {
        _itemList = SaveLoadSystem.Instance.InventoryItemList;
    }

    public List<Item> ListOfItem
    {
        get
        {
            List<Item> newList = new List<Item>();
            newList.AddRange(_itemList);
            return newList;
        }
    }
    public void AddItem(Item NewItem) => _itemList.Add(NewItem);
    public void RemoveItem(Item RemovedItem) => _itemList.Remove(RemovedItem);

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