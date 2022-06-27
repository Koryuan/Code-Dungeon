using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogBox : MonoBehaviour
{
    private bool canProceed = false;

    private int currentIndex;
    private DialogSetting currentSetting;

    public static DialogBox instance;
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
        instance = this;
    }
    private void CheckNullReferences()
    {
        if (!boxPanel) Debug.LogError("Dialog Box Panel is not Referenced");
        if (!namePanel) Debug.LogError("Dialog name Panel is not Referenced");
        if (!boxText) Debug.LogError("Dialog Box text is not Referenced");
        if (!nameText) Debug.LogError("Dialog name text is not Referenced");
    }
    #endregion

    #region Dialog Interaction
    public async void OpenDialog(DialogSetting dialog)
    {
        currentSetting = dialog;
        currentIndex = 0;

        NextDialog(currentSetting.Dialogs[currentIndex].Name, currentSetting.Dialogs[currentIndex].Text);
        
        await UniTask.Delay(100); 
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
    #endregion

    private void OnInterectInput(InputAction.CallbackContext Callback)
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

    #region Enable Disable
    private void OnEnable()
    {
        EnableInput();
    }
    private void OnDisable()
    {
        DisableInput();
    }
    private void EnableInput()
    {
        _interactionInput.action.performed += OnInterectInput;
    }
    private void DisableInput()
    {
        _interactionInput.action.performed -= OnInterectInput;
    }
    #endregion
}