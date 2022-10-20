using System;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject playGuidePanel;

    [Header("Button References")]
    [SerializeField] private MenuButton playButton;
    [SerializeField] private MenuButton loadButton;
    [SerializeField] private MenuButton optionButton;
    [SerializeField] private MenuButton quitButton;

    public MenuButton PlayButton => playButton;
    public MenuButton LoadButton => loadButton;
    public MenuButton OptionButton => optionButton;
    public MenuButton QuitButton => quitButton;
    
    public bool GuidePanelIsActive => playGuidePanel.activeSelf;

    public void Initialize(Action<MenuButton> OnSelectEvent)
    {
        CheckReferences();

        playButton.Button.OnSelectEvent += () => OnSelectEvent?.Invoke(playButton);
        loadButton.Button.OnSelectEvent += () => OnSelectEvent?.Invoke(loadButton);
        optionButton.Button.OnSelectEvent += () => OnSelectEvent?.Invoke(optionButton);
        quitButton.Button.OnSelectEvent += () => OnSelectEvent?.Invoke(quitButton);

        loadButton.SetHighlight(false);
        optionButton.SetHighlight(false);
        quitButton.SetHighlight(false);

        if (!SaveSystem.Instance.SaveFileExist) loadButton.Button.interactable = false;
    }
    private void CheckReferences()
    {
        if (!mainMenuPanel) Debug.LogError($"{name} has no panel references");
        if (!playGuidePanel) Debug.LogError($"{name} has no guide panel references");

        playButton.CheckReferences();
        loadButton.CheckReferences();
        optionButton.CheckReferences();
        quitButton.CheckReferences();
    }

    public void MainMenuPanel(bool Open) => mainMenuPanel.SetActive(Open);
    public void ActiveGuidePanel() => playGuidePanel.SetActive(true);

    #region Add listener to button
    public void AddPlayButtonListener(Action OnClick) => playButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    public void AddLoadButtonListener(Action OnClick) => loadButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    public void AddOptionButtonListener(Action OnClick) => optionButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    public void AddQuitButtonListener(Action OnClick) => quitButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    #endregion
}