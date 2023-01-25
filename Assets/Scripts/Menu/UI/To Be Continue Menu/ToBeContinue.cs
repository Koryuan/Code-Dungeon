using Cysharp.Threading.Tasks;
using UnityEngine;

public class ToBeContinue : MonoBehaviour, IPanelUI
{
    [Header("Button")]
    [SerializeField] private MenuButton m_exitButton;

    [Header("Channel")]
    [SerializeField] private MenuManager m_manager;
    [SerializeField] private LoadingChannel m_loadChannel;

    public void Open() => OpenPanel(null);

    public void OpenPanel(IMenuUI LastUI)
    {
        m_manager.OpenMenu(this,null);
        gameObject.SetActive(true);
    }

    private void ExitGame()
    {
        m_loadChannel.RaiseLoadingRequest();
        SceneLoad.LoadMainMenu();
    }

    async private void OnEnable()
    {
        m_exitButton.Button.onClick.AddListener(ExitGame);
        await UniTask.Delay(1000);

        m_exitButton.gameObject.SetActive(true);
        m_exitButton.Select();
        m_exitButton.SetHighlight(true);
    }
    private void OnDisable()
    {
        m_exitButton.Button.onClick.RemoveAllListeners();
        m_exitButton.gameObject.SetActive(true);
    }    
}