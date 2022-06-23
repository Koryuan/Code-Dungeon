using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Serializable] private class MainMenuButton
    {
        public Image ArrowImage;
        public HoverButton MenuButton;
    }

    private int menuButtonLength = 0;
    private int currentButton = 0;

    [Header("Button References")]
    [SerializeField] private MainMenuButton[] _MainMenuButton;

    [Header("Help References")]
    [SerializeField] private GameObject movementHelpPanel;

    [Header("Input References")]
    [SerializeField] private InputActionAsset playerActionMap;
    [SerializeField] private InputActionReference _movementInput;
    [SerializeField] private InputActionReference _interactionInput;

    #region Initialization
    private void Awake()
    {
        CheckNull();
        InitializeButton();
        InitializeInput();
    }
    private void CheckNull()
    {
        if (!movementHelpPanel) Debug.LogError("Movement Help Image is not Referenced");
    }
    private void InitializeButton()
    {
        int count = 0;
        foreach(MainMenuButton mainButton in _MainMenuButton)
        {
            if (count != 0) mainButton.ArrowImage.enabled = false;
            else mainButton.ArrowImage.enabled = true;

            switch(count)
            {
                case 0:
                    mainButton.MenuButton.onClick.AddListener(StartButton);
                    break;
                case 1:
                    mainButton.MenuButton.onClick.AddListener(OptionButton);
                    break;
                case 2:
                    mainButton.MenuButton.onClick.AddListener(ExitButton);
                    break;
                default:
                    Debug.Log($"There is no info for number {count}");
                    break;
            }

            mainButton.MenuButton.buttonNumber = count;
            mainButton.MenuButton.OnPointerHover += MoveCurrentButton;
            count++;
        }

        menuButtonLength = count;
        if (menuButtonLength == 0) Debug.LogError("Button is not referenced");
    }
    private void InitializeInput()
    {
        _interactionInput.action.performed +=  (a) => OnInterectInput();
        _movementInput.action.performed += (a) => OnMenuMovementInput();
    }
    #endregion

    #region Button Function
    private void StartButton()
    {
        Debug.Log("Open Help UI");
        movementHelpPanel.SetActive(true);
    }

    private void OptionButton()
    {
        Debug.Log("Open Option Menu");
    }

    private void ExitButton()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    private void MoveCurrentButton(int buttonNumber)
    {
        if (buttonNumber > menuButtonLength - 1) buttonNumber = 0;
        else if (buttonNumber < 0) buttonNumber = menuButtonLength - 1;

        _MainMenuButton[currentButton].ArrowImage.enabled = false;
        _MainMenuButton[buttonNumber].ArrowImage.enabled = true;

        currentButton = buttonNumber;
    }
    #endregion

    private void OnInterectInput()
    {
        if (movementHelpPanel.activeSelf) SceneLoadManager.LoadTutorialMap();
        else _MainMenuButton[currentButton].MenuButton.onClick.Invoke();
    }
    private void OnMenuMovementInput()
    {
        if (!movementHelpPanel.activeSelf)
        {
            float movePosition = _movementInput.action.ReadValue<float>();
            if (movePosition > 0) MoveCurrentButton(currentButton + 1);
            else MoveCurrentButton(currentButton - 1);
        }
    }

    #region Enable Disable
    private void OnEnable()
    {
        playerActionMap.Enable();
    }

    private void OnDisable()
    {
        playerActionMap.Disable();
    }
    #endregion
}
