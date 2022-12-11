using Cysharp.Threading.Tasks;
using UnityEngine;

public class SaveChanneler : MonoBehaviour
{
    [SerializeField] private HelpChannel m_helpChannel;
    [SerializeField] private ItemChannel m_itemChannel;

    async private void Awake()
    {
        await UniTask.WaitUntil(() => SaveLoadSystem.Instance && SaveLoadSystem.Instance._SaveData != null);

        // Help Channel
        m_helpChannel.OnHelpInserted += SaveLoadSystem.Instance._SaveData.HelpList.Add;
        m_helpChannel.OnHelpListRequested += OnRequestHelpList;

        // Item Channel
        m_itemChannel.OnItemInserted += SaveLoadSystem.Instance._SaveData._ItemList.Add;
        m_itemChannel.OnItemListRequested += OnRequestItemList;
        m_itemChannel.OnItemRemoved += RemoveItemFromList;
    }

    private void RemoveItemFromList(Item RemovedItem) => SaveLoadSystem.Instance._SaveData._ItemList.Remove(RemovedItem);

    private Item[] OnRequestItemList()
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        Debug.Log($"{name}, send item list data");
        return SaveLoadSystem.Instance._SaveData._ItemList.ToArray();
    }

    private HelpSettings[] OnRequestHelpList()
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        Debug.Log($"{name}, send help list data");
        return SaveLoadSystem.Instance._SaveData.HelpList.ToArray();
    }

    private void OnDestroy()
    {
        // Help
        m_helpChannel.OnHelpInserted -= SaveLoadSystem.Instance._SaveData.HelpList.Add;
        m_helpChannel.OnHelpListRequested -= OnRequestHelpList;

        // Item
        m_itemChannel.OnItemInserted -= SaveLoadSystem.Instance._SaveData._ItemList.Add;
        m_itemChannel.OnItemListRequested -= OnRequestItemList;
        m_itemChannel.OnItemRemoved -= RemoveItemFromList;
    }
}