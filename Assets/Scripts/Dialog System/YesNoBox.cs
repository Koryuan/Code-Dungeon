using System;
using UnityEngine;
using UnityEngine.UI;

public class YesNoBox : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject YesNoBoxPanel;

    [Header("Button")]
    [SerializeField] private YesNoButton yesButton;
    [SerializeField] private YesNoButton noButton;

    public event Action onYesButton;
    public event Action onNoButton;

    public bool IsYes => yesButton.Arrow.enabled;
    public bool IsOpen => YesNoBoxPanel.activeSelf;

    private void Awake()
    {
        CheckNullReferences();
        yesButton.Button.OnSelectHover += ChangeToYes;
        noButton.Button.OnSelectHover += ChangeToNo;
    }
    private void CheckNullReferences()
    {
        if (!YesNoBoxPanel) Debug.LogError($"{name} has no box panel references");
        yesButton.CheckNullReferences("Yes Button");
        noButton.CheckNullReferences("No Button");
    }

    public void OpenYesNoBox()
    {
        ChangeToYes();
        YesNoBoxPanel.SetActive(true);
    }
    public void CloseYesNoBox() => YesNoBoxPanel.SetActive(false);
    public void SelectButton()
    {
        if (IsYes) onYesButton?.Invoke();
        else onNoButton?.Invoke();
    }

    #region Change current arrow position
    public void ChangeCurrentButton()
    {
        yesButton.Arrow.enabled = !yesButton.Arrow.enabled;
        noButton.Arrow.enabled = !noButton.Arrow.enabled;
    }
    private void ChangeToNo()
    {
        yesButton.Arrow.enabled = false;
        noButton.Arrow.enabled = true;
    }
    private void ChangeToYes()
    {
        yesButton.Arrow.enabled = true;
        noButton.Arrow.enabled = false;
    }
    #endregion
}

[Serializable] public class YesNoButton
{
    public Image Arrow;
    public HoverButton Button;
    
    public void CheckNullReferences(string Name)
    {
        if (!Arrow) Debug.LogError($"{Name} has no arrow image references");
        if (!Button) Debug.LogError($"{Name} has no button references");
    }
}