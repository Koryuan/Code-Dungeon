using UnityEngine;

public class Loading : MonoBehaviour
{
    private static Loading Instance;

    [SerializeField] private LoadingChannel m_channel;
    [SerializeField] private GameObject m_LoadingUI;

    private bool GameManagerLoading { get; set; } = false;
    private bool LoadSceneLoading { get; set; } = false;
    private bool m_allLoaded => !GameManagerLoading && !LoadSceneLoading;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        else Instance = this;
        DontDestroyOnLoad(this);

        // Channel Setup
        m_channel.OnLoadUpdated += LoadingFinish;
        m_channel.OnRequestLoading += StartLoading;
    }

    public void StartLoading()
    {
        LoadSceneLoading = GameManagerLoading = true;
        m_LoadingUI.SetActive(true);
    }
    public void LoadingFinish(LoadingType type)
    {
        switch (type)
        {
            case LoadingType.GameManager:
                GameManagerLoading = false;
                break;
            case LoadingType.LoadScene:
                LoadSceneLoading = false;
                break;
            default:
                break;
        }
        if (m_allLoaded)
        {
            m_channel.RaiseLoadingFinish();
            m_LoadingUI.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Channel Setup
        m_channel.OnLoadUpdated -= LoadingFinish;
        m_channel.OnRequestLoading -= StartLoading;
    }
}

public enum LoadingType
{
    GameManager,
    LoadScene
}