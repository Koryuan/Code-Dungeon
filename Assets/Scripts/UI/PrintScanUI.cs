using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PrintScanUI : MonoBehaviour
{
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

    #region 
    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        
    }
    private void EnableInput()
    {

    }
    private void DisableInput()
    {

    }
    #endregion
}