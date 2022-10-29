using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    #region Instance
    private static TimeManager m_instance;
    public static TimeManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("Time").AddComponent<TimeManager>();
                DontDestroyOnLoad(m_instance.gameObject);
            }
            return m_instance;
        }
    }
    #endregion

    private bool isInitialize = false;

    async private void Awake()
    {
        await UniTask.WaitUntil(()=> SaveLoadSystem.Instance._SaveData != null);
        SceneManager.sceneLoaded += OnSceneLoad;
        isInitialize = true;
    }

    private void Update()
    {
        if (isInitialize) SaveLoadSystem.Instance._SaveData.PlayTime += Time.deltaTime;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
}