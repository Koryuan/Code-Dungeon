using System;
using UnityEngine;

public class OptionMenuUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject optionMenuPanel;

    [Header("Slider References")]
    [SerializeField] private MenuSlider sfxSlider;
    [SerializeField] private MenuSlider bgmSlider;

    [Header("Button References")]
    [SerializeField] private MenuButton exitButton;

    public MenuSlider SFXSlider => sfxSlider;
    public bool IsOpen => optionMenuPanel.activeSelf;
    #region Initialization
    public void Initialize(Action<IMenuUI> OnSelectEvent)
    {
        CheckReferences();

        sfxSlider.Slider.OnSelectHover += () => OnSelectEvent?.Invoke(sfxSlider);
        bgmSlider.Slider.OnSelectHover += () => OnSelectEvent?.Invoke(bgmSlider);
        exitButton.Button.OnSelectEvent += () => OnSelectEvent?.Invoke(exitButton);

        sfxSlider.SetHighlight(false);
        bgmSlider.SetHighlight(false);
        exitButton.SetHighlight(false);
    }
    private void CheckReferences()
    {
        exitButton.CheckReferences();
    }
    #endregion

    public void OptionMenu(bool Open) => optionMenuPanel.SetActive(Open);

    public void AddExitButtonListener(Action OnClick) => exitButton.Button.onClick.AddListener(() => OnClick?.Invoke());
}