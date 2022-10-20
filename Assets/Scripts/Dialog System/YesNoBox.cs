using System;
using UnityEngine;
using UnityEngine.UI;

public class YesNoBox : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject Panel;

    [Header("Button")]
    [SerializeField] private YesNoButton yesButton;
    [SerializeField] private YesNoButton noButton;

    public event Action onYesButton;
    public event Action onNoButton;

    #region Initialization
    private void Awake()
    {
        CheckNullReferences();

        yesButton.SetHighlight(false);
        yesButton.Button.OnSelectEvent += () => ChangeYesNo(true);
        yesButton.Button.onClick.AddListener(()=> onYesButton?.Invoke());

        noButton.SetHighlight(false);
        noButton.Button.OnSelectEvent += () => ChangeYesNo(false);
        noButton.Button.onClick.AddListener(() => onNoButton?.Invoke());
    }
    private void CheckNullReferences()
    {
        if (!Panel) Debug.LogError($"{name} has no box panel references");
        yesButton.CheckNullReferences("Yes Button");
        noButton.CheckNullReferences("No Button");
    }
    #endregion
    public void OpenYesNo(Action OnYesButton, Action OnNoButton)
    {
        Panel.SetActive(true);
        yesButton.Select();

        onYesButton = onNoButton = ClosePanel;
        onYesButton += OnYesButton; onNoButton += OnNoButton;
    }
    public void ChangeYesNo(bool IsYes)
    {
        yesButton.SetHighlight(IsYes);
        noButton.SetHighlight(!IsYes);
    }
    public void ClosePanel() => Panel.SetActive(false);
}

[Serializable] public class YesNoButton : IMenuUI
{
    public Image Arrow;
    public HoverButton Button;
    
    public void CheckNullReferences(string Name)
    {
        if (!Arrow) Debug.LogError($"{Name} has no arrow image references");
        if (!Button) Debug.LogError($"{Name} has no button references");
    }

    public void Select() => Button.Select();
    public void SetHighlight(bool IsHighlighted) => Arrow.enabled = IsHighlighted;
}