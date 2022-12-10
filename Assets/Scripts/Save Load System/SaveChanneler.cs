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

    private HelpSettings[] OnRequestHelpList()
    {
        if (!SaveLoadSystem.Instance || SaveLoadSystem.Instance._SaveData == null) return null;

        Debug.Log($"{name}, send help data");
        return SaveLoadSystem.Instance._SaveData.HelpList.ToArray();
    }

    private void OnDestroy()
    {
        m_helpChannel.OnHelpInserted -= SaveLoadSystem.Instance._SaveData.HelpList.Add;
        m_helpChannel.OnHelpRequested -= OnRequestHelpList;
    }
}