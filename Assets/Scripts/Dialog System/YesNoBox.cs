using System;
using UnityEngine;
using UnityEngine.UI;

public class YesNoBox : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Button")]
    [SerializeField] private YesNoButton yesButton;
    [SerializeField] private YesNoButton noButton;

    public event Action OnYes;
    public event Action OnNo;

    #region Initialization
    private void Awake()
    {
        CheckReferences();

        yesButton.SetHighlight(false);
        yesButton.Button.OnSelectEvent += () => ChangeYesNo(true);
        yesButton.Button.onClick.AddListener(()=> OnYes?.Invoke());

        noButton.SetHighlight(false);
        noButton.Button.OnSelectEvent += () => ChangeYesNo(false);
        noButton.Button.onClick.AddListener(() => OnNo?.Invoke());
    }
    private void CheckReferences()
    {
        if (!panel) Debug.LogError($"{name} has no box panel references");
        yesButton.CheckReferences("Yes Button");
        noButton.CheckReferences("No Button");
    }
    #endregion

    public void OpenYesNo(Action OnYesButton, Action OnNoButton)
    {
        panel.SetActive(true);
        yesButton.Select();

        OnYes = OnNo = ClosePanel;
        OnYes += OnYesButton; OnNo += OnNoButton;
    }
    private void ChangeYesNo(bool IsYes)
    {
        yesButton.SetHighlight(IsYes);
        noButton.SetHighlight(!IsYes);
    }
    private void ClosePanel() => panel.SetActive(false);
}

[Serializable] public class YesNoButton : IMenuUI
{
    public Image Arrow;
    public HoverButton Button;
    
    public void CheckReferences(string Name)
    {
        if (!Arrow) Debug.LogError($"{Name} has no arrow image references");
        if (!Button) Debug.LogError($"{Name} has no button references");
    }

    public void Select()
    {
        Button.Select();
    }
    public void SetHighlight(bool IsHighlighted) => Arrow.enabled = IsHighlighted;
}