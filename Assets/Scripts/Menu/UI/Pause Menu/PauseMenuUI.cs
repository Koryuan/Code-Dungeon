using System;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject pauseMenuPanel;

    [Header("Button References")]
    [SerializeField] private MenuButton resumeButton;
    [SerializeField] private MenuButton optionButton;
    [SerializeField] private MenuButton loadButton;
    [SerializeField] private MenuButton exitButton;

    public MenuButton ResumeButton => resumeButton;
    public MenuButton LoadButton => loadButton;
    public MenuButton OptionButton => optionButton;
    public MenuButton ExitButton => exitButton;

    public void Initialize(Action<MenuButton> OnSelectEvent)
    {
        CheckReferences();

        resumeButton.Button.OnSelectEvent += () => OnSelectEvent?.Invoke(resumeButton);
        loadButton.Button.OnSelectEvent += () => OnSelectEvent?.Invoke(loadButton);
        optionButton.Button.OnSelectEvent += () => OnSelectEvent?.Invoke(optionButton);
        exitButton.Button.OnSelectEvent += () => OnSelectEvent?.Invoke(exitButton);

        resumeButton.SetHighlight(false);
        loadButton.SetHighlight(false);
        optionButton.SetHighlight(false);
        exitButton.SetHighlight(false);

        if (!SaveLoadSystem.Instance.SaveFileExist) loadButton.Button.interactable = false;
    }

    private void CheckReferences()
    {
        if (!pauseMenuPanel) Debug.LogError($"{name} has no pause menu panel");
        resumeButton.CheckReferences();
        loadButton.CheckReferences();
        optionButton.CheckReferences();
        exitButton.CheckReferences();
    }

    public void PauseMenuPanel(bool Open) => pauseMenuPanel.SetActive(Open);

    public void UpdateLoadButton()
    {
        if (!SaveLoadSystem.Instance.SaveFileExist) loadButton.Button.interactable = false;
    }

    public void AddResumeButtonListener(Action OnClick) => resumeButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    public void AddLoadButtonListener(Action OnClick) => loadButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    public void AddOptionButtonListener(Action OnClick) => optionButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    public void AddExitButtonListener(Action OnClick) => exitButton.Button.onClick.AddListener(() => OnClick?.Invoke());
}