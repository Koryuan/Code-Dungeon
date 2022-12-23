using System;
using System.Collections;
using UnityEngine;

[System.Serializable] public class GameOverMenuUI
{
    [Header("Holder")]
    [SerializeField] private GameObject m_panel;

    [Header("Button")]
    [SerializeField] private MenuButton m_loadButton;
    [SerializeField] private MenuButton m_exitButton;

    public MenuButton LoadButton => m_loadButton;

    #region Initialization
    public void Initialize()
    {
        CheckReference();

        m_loadButton.Button.OnSelectEvent += SelectLoadButton;
        m_loadButton.Button.OnDeselectEvent += m_loadButton.SetHighlight;
        m_loadButton.SetHighlight(false);

        m_exitButton.Button.OnSelectEvent += SelectExitButton;
        m_exitButton.Button.OnDeselectEvent += m_exitButton.SetHighlight;
        m_exitButton.SetHighlight(false);
    }
    private void CheckReference()
    {
        if (!m_panel) Debug.LogError("Game Over Menu, has no panel to open");
        if (!m_loadButton) Debug.LogError("Game Over Menu, has no Load button UI");
        if (!m_exitButton) Debug.LogError("Game Over Menu, has no Exit button UI");
    }
    #endregion

    public void OpenPanel(bool Open) => m_panel.SetActive(Open);

    public void SelectLoadButton() => m_loadButton.SetHighlight(true);
    public void SelectExitButton() => m_exitButton.SetHighlight(true);

    #region Add listener to button
    public void AddLoadButtonListener(Action OnClick) => m_loadButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    public void AddExitButtonListener(Action OnClick) => m_exitButton.Button.onClick.AddListener(() => OnClick?.Invoke());
    #endregion
}