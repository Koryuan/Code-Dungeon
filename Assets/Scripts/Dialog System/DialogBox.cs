using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogBox : MonoBehaviour
{
    private bool canProceed = false;

    private int currentIndex;
    private DialogSetting currentSetting;

    public Action OnDialogBoxOpen;
    public Action OnDialogBoxClose;

    [Header("Input References")]
    [SerializeField] private InputActionReference _interactionInput;

    [Header("References")]
    [SerializeField] private GameObject namePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject boxPanel;
    [SerializeField] private TMP_Text boxText;

    #region Initialization
    private void Awake()
    {
        CheckNullReferences();
        InitializeInput();
    }
    private void CheckNullReferences()
    {
        if (!boxPanel) Debug.LogError("Dialog Box Panel is not Referenced");
        if (!namePanel) Debug.LogError("Dialog name Panel is not Referenced");
        if (!boxText) Debug.LogError("Dialog Box text is not Referenced");
        if (!nameText) Debug.LogError("Dialog name text is not Referenced");
    }
    private void InitializeInput()
    {
        _interactionInput.action.performed += (a) => OnInterectInput();
    }
    #endregion

    private void OnInterectInput()
    {
        if (canProceed)
        {
            if (currentIndex == currentSetting.Dialogs.Length - 1)
            {
                canProceed = false;
                CloseDialog();
            }
            else
            {
                currentIndex++;
                NextDialog(currentSetting.Dialogs[currentIndex].Name, currentSetting.Dialogs[currentIndex].Text);
            }
        }
    }

    public void OpenDialog(DialogSetting dialog)
    {
        currentSetting = dialog;
        currentIndex = 0;

        NextDialog(currentSetting.Dialogs[currentIndex].Name, currentSetting.Dialogs[currentIndex].Text);
        canProceed = true;

        OnDialogBoxOpen?.Invoke();
    }
    private void NextDialog(string name, string text)
    {
        nameText.text = name;
        boxText.text = text;

        if (string.IsNullOrEmpty(name)) namePanel.SetActive(false);
        else namePanel.SetActive(true);
        boxPanel.SetActive(true);
    }
    private void CloseDialog()
    {
        namePanel.SetActive(false);
        boxPanel.SetActive(false);
        OnDialogBoxClose?.Invoke();
    }
}