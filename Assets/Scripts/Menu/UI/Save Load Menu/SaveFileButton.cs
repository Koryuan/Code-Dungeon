using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HoverButton))]
public class SaveFileButton : MonoBehaviour, IMenuUI
{
    #region Enumerator
    private enum State
    {
        Save,
        Load
    }
    #endregion

    [Header("Text")]
    [SerializeField] private TMP_Text safeFileName;
    [SerializeField] private TMP_Text description;

    [Header("Highlight")]
    [SerializeField] private Image arrow;

    private HoverButton button;
    private SaveData _saveData;
    private string _saveDataName;
    private State _state = State.Save;

    #region Initialization
    public void Initialize(SaveData NewSaveData, string SaveDataName)
    {
        button = GetComponent<HoverButton>();
        _saveDataName = SaveDataName;

        CheckReferences();
        UpdateSaveData(NewSaveData);
        button.onClick.AddListener(SaveLoad);
        button.OnSelectEvent += () => SetHighlight(true);
        button.OnDeselectEvent += SetHighlight;
    }
    private void CheckReferences()
    {
        if (!safeFileName) Debug.LogError($"{name} has no Name text references");
        if (!description) Debug.LogError($"{name} has no Description text references");
        if (!button) Debug.LogError($"{name} has no Button references");
    }
    #endregion

    public void SetHighlight(bool IsHighlighted) => arrow.enabled = IsHighlighted;
    public void Select() => button.Select();

    public void UpdateState(bool IsSave) => _state = IsSave ? State.Save : State.Load;

    #region Save
    private void SaveLoad()
    {
        bool canUse = true;
        if (GameManager.Instance) canUse = GameManager.Instance.CurrentState == GameState.Game_Save_Load_State;
        if (_state == State.Save && canUse)
        {
            UpdateSaveData(SaveLoadSystem.Instance._SaveData);
            SaveLoadSystem.Instance.SaveFile(_saveDataName);
        }
        else if (canUse)
        {
            SaveLoadSystem.Instance.LoadData(_saveDataName);
        }
    }
    private void UpdateSaveData(SaveData NewSaveData)
    {
        if (NewSaveData == null) return;

        _saveData = NewSaveData;
        safeFileName.text = NewSaveData.SaveFileName;
        UpdateDescription(NewSaveData.PlayTimeText, NewSaveData.LastChapterName);
    }
    private void UpdateDescription(string PlayTime, string LastChapter)
        => description.text = $"Play time\t: {PlayTime}\nLast chapter\t: {LastChapter}";
    #endregion
}