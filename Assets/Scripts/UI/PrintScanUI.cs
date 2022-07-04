using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PrintScanUI : MonoBehaviour
{
    private bool canInterect => GameManager.Instance.CurrentState == GameState.PlayerControl;
    private bool canPrintInterect => canInterect && printButton.ImageHolder.sprite == printButton.Highlight;
    private bool canScanInterect => canInterect && scanButton.ImageHolder.sprite == scanButton.Highlight;

    [Header("Button References")]
    [SerializeField] private PrintScanButton printButton;
    [SerializeField] private PrintScanButton scanButton;

    [Header("Input References")]
    [SerializeField] private InputActionReference printInput;
    [SerializeField] private InputActionReference scanInput;

    [System.Serializable] private class PrintScanButton
    {
        public Sprite Highlight;
        public Sprite NonHighlight;
        public Image ImageHolder;
    }

    public void OpenPrintScan(bool PrintOpen, bool ScanOpen)
    {
        if (PrintOpen) printButton.ImageHolder.sprite = printButton.Highlight;
        else printButton.ImageHolder.sprite = printButton.NonHighlight;
        if (ScanOpen) scanButton.ImageHolder.sprite = scanButton.Highlight;
        else scanButton.ImageHolder.sprite = scanButton.NonHighlight;
    }
    public void ClosePrintScan()
    {
        printButton.ImageHolder.sprite = printButton.NonHighlight;
        scanButton.ImageHolder.sprite = scanButton.NonHighlight;
    }

    private void PrintInterect(InputAction.CallbackContext Callback)
    {
        if (canPrintInterect) InteractionManager.Instance.PrintInterectTarget();
    }
    private void ScanInterect(InputAction.CallbackContext Callback)
    {
        if (canScanInterect) InteractionManager.Instance.ScanInterectTarget();
    }

    #region Enable Disable
    private void OnEnable()
    {
        printInput.action.performed += PrintInterect;
        scanInput.action.performed += ScanInterect;
    }
    private void OnDisable()
    {
        printInput.action.performed -= PrintInterect;
        scanInput.action.performed -= ScanInterect;
    }
    #endregion
}