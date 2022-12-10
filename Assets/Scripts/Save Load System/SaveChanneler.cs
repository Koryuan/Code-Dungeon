using Cysharp.Threading.Tasks;
using UnityEngine;

public class SaveChanneler : MonoBehaviour
{
    [SerializeField] private HelpChannel m_helpChannel;

    async private void Awake()
    {
        await UniTask.WaitUntil(() => SaveLoadSystem.Instance && SaveLoadSystem.Instance._SaveData != null);
        m_helpChannel.OnHelpInserted += SaveLoadSystem.Instance._SaveData.HelpList.Add;
        m_helpChannel.OnHelpRequested += OnRequestHelpList;
    }

    private void OnRequestHelpList()
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return;
        
        m_helpChannel.OnHelpInsertedMultiple(SaveLoadSystem.Instance._SaveData.HelpList.ToArray());
        Debug.Log("Data send");
    }

    private void OnDestroy()
    {
        m_helpChannel.OnHelpInserted -= SaveLoadSystem.Instance._SaveData.HelpList.Add;
        m_helpChannel.OnHelpRequested -= OnRequestHelpList;
    }
}