using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Serializable] private class MainMenuButton
    {
        public Image ArrowImage;
        public Sprite Highlight;
        public Sprite NonHighlight;
        public HoverButton MenuButton;

        public bool CheckNullReferences()
        {
            if (!ArrowImage)
            {
                Debug.LogError("There is no Arrow Image References");
                return false;
            }
            else if (!Highlight)
            {
                Debug.LogError("There is no Highlight sprite References");
                return false;
            }
            else if (!NonHighlight)
            {
                Debug.LogError("There is no Nonhighlight sprite References");
                return false;
            }
            else if (!MenuButton)
            {
                Debug.LogError("There is no Hover Button References");
                return false;
            }
            return true;
        }
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
            if (!mainButton.CheckNullReferences()) Debug.LogError($"Button number:{count+1}, has null References");
            if (count != 0)
            {
                mainButton.ArrowImage.enabled = false;
                mainButton.MenuButton.image.sprite = mainButton.NonHighlight;
            }
            else
            {
                mainButton.ArrowImage.enabled = true;
                mainButton.MenuButton.image.sprite = mainButton.Highlight;
            }

            switch(count)
            {
                case 0:
                    mainButton.MenuButton.onClick.AddListener(StartButton);
                    break;
                case 1:
                    mainButton.MenuButton.onClick.AddListener(LoadButton);
                    mainButton.MenuButton.interactable = SaveSystem.Instance.SaveFileExist;
                    break;
                case 2:
                    mainButton.MenuButton.onClick.AddListener(OptionButton);
                    break;
                case 3:
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
    #endregion

    #region Button Function
    private void StartButton()
    {
        Debug.Log("Open Help UI");
        movementHelpPanel.SetActive(true);
    }

    private void LoadButton()
    {
        Debug.Log("Load Game");
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
        if (buttonNumber == 1 && !SaveSystem.Instance.SaveFileExist) buttonNumber = currentButton > buttonNumber ? 0 : 2;
        else if (buttonNumber > menuButtonLength - 1) buttonNumber = 0;
        else if (buttonNumber < 0) buttonNumber = menuButtonLength - 1;

        _MainMenuButton[currentButton].ArrowImage.enabled = false;
        _MainMenuButton[currentButton].MenuButton.image.sprite = _MainMenuButton[currentButton].NonHighlight;
        _MainMenuButton[buttonNumber].ArrowImage.enabled = true;
        _MainMenuButton[buttonNumber].MenuButton.image.sprite = _MainMenuButton[buttonNumber].Highlight;

        currentButton = buttonNumber;
    }
    #endregion

    private void OnInterectInput(InputAction.CallbackContext callback)
    {
        if (movementHelpPanel.activeSelf) SceneLoadManager.LoadTutorialMap();
        else _MainMenuButton[currentButton].MenuButton.onClick.Invoke();
    }
    private void OnMenuMovementInput(InputAction.CallbackContext callback)
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
        InitializeInput();
    }

    private void OnDisable()
    {
        playerActionMap.Disable();
        UnRefInput();
    }
    private void InitializeInput()
    {
        _interactionInput.action.performed += OnInterectInput;
        _movementInput.action.performed += OnMenuMovementInput;
    }
    private void UnRefInput()
    {
        _interactionInput.action.performed -= OnInterectInput;
        _movementInput.action.performed -= OnMenuMovementInput;
    }
    #endregion
}