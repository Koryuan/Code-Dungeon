﻿using System;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private MainMenuButton playButton;
    [SerializeField] private MainMenuButton loadButton;
    [SerializeField] private MainMenuButton optionButton;
    [SerializeField] private MainMenuButton quitButton;

    public MainMenuButton PlayButton => playButton;
    public MainMenuButton LoadButton => loadButton;
    public MainMenuButton OptionButton => optionButton;
    public MainMenuButton QuitButton => quitButton;

    [Header("Help References")]
    [SerializeField] private GameObject playGuidePanel;
    public bool GuidePanelIsActive => playGuidePanel.activeSelf;

    public void Initialize(Action<MainMenuButton> OnSelectEvent)
    {
        CheckReferences();

        playButton.Button.OnSelectHover += () => OnSelectEvent?.Invoke(playButton);
        loadButton.Button.OnSelectHover += () => OnSelectEvent?.Invoke(loadButton);
        optionButton.Button.OnSelectHover += () => OnSelectEvent?.Invoke(optionButton);
        quitButton.Button.OnSelectHover += () => OnSelectEvent?.Invoke(quitButton);

        loadButton.ChangeHighlight(false);
        optionButton.ChangeHighlight(false);
        quitButton.ChangeHighlight(false);

        if (!SaveSystem.Instance.SaveFileExist) loadButton.Button.interactable = false;
    }
    private void CheckReferences()
    {
        playButton.CheckReferences();
        loadButton.CheckReferences();
        optionButton.CheckReferences();
        quitButton.CheckReferences();
    }

    public void ActiveGuidePanel() => playGuidePanel.SetActive(true);

    #region Add listener to button
    public void AddPlayButtonListener(Action OnClick) => playButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    public void AddLoadButtonListener(Action OnClick) => loadButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    public void AddOptionButtonListener(Action OnClick) => optionButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    public void AddQuitButtonListener(Action OnClick) => quitButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    #endregion
}