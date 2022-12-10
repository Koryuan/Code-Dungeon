using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogBox : MonoBehaviour
{
    private bool canProceed = false;

    private int currentIndex;
    private DialogSetting currentDialog;

    public Action OnDialogBoxClose;

    [Header("References")]
    [SerializeField] private GameObject namePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject boxPanel;
    [SerializeField] private TMP_Text boxText;

    #region Initialization
    private void Awake()
    {
        CheckNullReferences();
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
        currentDialog = dialog;
        currentIndex = 0;

        NextDialog(currentDialog.Dialogs[currentIndex], true);

        // Exception Handeller
        var cts = this.GetCancellationTokenOnDestroy();
        try{
            await UniTask.Delay(100, cancellationToken: cts);
            canProceed = true;
        }
        catch (Exception){
            Debug.LogError($"{name} got destroyed while still open");
        }
    }
    private void NextDialog(Dialog Next, bool JustOpen = false)
    {
        (nameText.text, boxText.text) = Next.DialogDetail;

        if (AudioManager.Instance && !JustOpen) AudioManager.Instance.PlayUIConfirm();

        if (string.IsNullOrEmpty(Next.DialogDetail.Name)) namePanel.SetActive(false);
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

    #region Input References
    private InputActionReference interactionInput => InputReferences.Instance._Menu_Interect;
    private void OnInterectInput(InputAction.CallbackContext Callback)
    {
        if (canProceed)
        {
            if (currentIndex == currentDialog.Dialogs.Length - 1)
            {
                canProceed = false;
                CloseDialog();
            }
            else
            {
                currentIndex++;
                NextDialog(currentDialog.Dialogs[currentIndex]);
            }
        }
    }
    #endregion

    #region Enable Disable
    async private void OnEnable()
    {
        await UniTask.WaitUntil(()=> InputReferences.Instance);
        interactionInput.action.performed += OnInterectInput;
    }
    async private void OnDisable()
    {
        await UniTask.WaitUntil(() => InputReferences.Instance);
        interactionInput.action.performed -= OnInterectInput;
    }
    #endregion
}