using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GuidePanel : MonoBehaviour
{
    private GuideContent content;
    private int currentContentPage;
    private bool canControl => GameManager.Instance.CurrentState == GameState.GuidePanelOpen;

    public Action OnClosePanel;

    [Header("Guide Panel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Image image;

    [Header("Button References")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    [Header("Input References")]
    [SerializeField] private InputActionReference _nextInput;

    #region Initialization
    private void Awake()
    {
        CheckNullReferences();
        InitializeButton();
    }
    private void CheckNullReferences()
    {
        if (!nextButton) Debug.LogError($"{name} has no button to go next");
        if (!prevButton) Debug.LogError($"{name} has no button to go back");
        if (!_nextInput) Debug.LogError($"{name} has no input references to go next");
    }
    private void InitializeButton()
    {
        nextButton.onClick.AddListener(NextContent);
        prevButton.onClick.AddListener(PrevContent);
    }
    #endregion

    public void OpenGuide(GuideContent NewContent)
    {
        content = NewContent;
        currentContentPage = -1;
        NextContent();
        gameObject.SetActive(true);
    }
    private void NextContent()
    {
        if (currentContentPage != content.GuideImage.Length - 1 && canControl)
        {
            currentContentPage++;
            UpdateImage();
        }
    }
    private void PrevContent()
    {
        Debug.Log("Jalan");
        if (currentContentPage != 0 && canControl)
        {
            currentContentPage--;
            UpdateImage();
        }
    }
    private void UpdateImage()
    {
        image.sprite = content.GuideImage[currentContentPage];
        nextButton.gameObject.SetActive(true);
        prevButton.gameObject.SetActive(true);
        if (currentContentPage == content.GuideImage.Length - 1) nextButton.gameObject.SetActive(false);
        if (currentContentPage == 0) prevButton.gameObject.SetActive(false);
    }
    private void CloseGuide()
    {
        panel.SetActive(false);
        OnClosePanel?.Invoke();
    }
    private void NextContent(InputAction.CallbackContext Callback)
    {
        if (currentContentPage == content.GuideImage.Length - 1) CloseGuide();
        else NextContent();
    }

    #region Enable Disable
    private void OnEnable()
    {
        _nextInput.action.performed += NextContent;
    }
    private void OnDisable()
    {
        _nextInput.action.performed -= NextContent;
    }
    #endregion
}