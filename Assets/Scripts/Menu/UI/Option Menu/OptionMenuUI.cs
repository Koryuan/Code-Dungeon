using Cysharp.Threading.Tasks;
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
    async public void Initialize(Action<IMenuUI> OnSelectEvent)
    {
        CheckReferences();

        sfxSlider.Slider.OnSelectHover += () => OnSelectEvent?.Invoke(sfxSlider);
        bgmSlider.Slider.OnSelectHover += () => OnSelectEvent?.Invoke(bgmSlider);
        exitButton.Button.OnSelectEvent += () => OnSelectEvent?.Invoke(exitButton);

        sfxSlider.SetHighlight(false);
        bgmSlider.SetHighlight(false);
        exitButton.SetHighlight(false);

        sfxSlider.Slider.onValueChanged.AddListener(UpdateSFXVolume);
        bgmSlider.Slider.onValueChanged.AddListener(UpdateBGMVolume);

        await UniTask.WaitUntil(() => SaveLoadSystem.Instance?._MasterData != null);

        sfxSlider.Slider.value = SaveLoadSystem.Instance._MasterData.SFXVolume;
        bgmSlider.Slider.value = SaveLoadSystem.Instance._MasterData.BGMVolume;
    }
    private void CheckReferences()
    {
        exitButton.CheckReferences();
    }
    #endregion

    private void UpdateSFXVolume(float Value)
    {
        if (SaveLoadSystem.Instance?._SaveData != null) SaveLoadSystem.Instance._MasterData.SFXVolume = Value;
    }
    private void UpdateBGMVolume(float Value)
    {
        if (SaveLoadSystem.Instance?._SaveData != null) SaveLoadSystem.Instance._MasterData.BGMVolume = Value;
    }

    public void OptionMenu(bool Open) => optionMenuPanel.SetActive(Open);

    public void AddExitButtonListener(Action OnClick) => exitButton.Button.onClick.AddListener(() => OnClick?.Invoke());

    private void OnDestroy()
    {
        if (sfxSlider) sfxSlider.Slider.onValueChanged.RemoveAllListeners();
        if (bgmSlider) bgmSlider.Slider.onValueChanged.RemoveAllListeners();
    }
}