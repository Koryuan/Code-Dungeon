using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject pauseMenuPanel;

    [Header("Slider References")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;

    [Header("Button References")]
    [SerializeField] private MenuButton exitButton;

    public void Initialize(Action<MenuButton> OnSelectEvent)
    {
        CheckReferences();
        exitButton.Button.OnSelectHover += () => OnSelectEvent?.Invoke(exitButton);
    }
    private void CheckReferences()
    {
        exitButton.CheckReferences();
    }
}