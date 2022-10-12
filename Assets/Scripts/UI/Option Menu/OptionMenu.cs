using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    private MenuButton currentButton = null;

    [Header("Main References")]
    [SerializeField] private OptionMenuUI _UI;
    [SerializeField] private MenuButton previousButton;

    private void Awake()
    {
        CheckReferences();
        InitializeUI();
    }

    private void CheckReferences()
    {
        if (!_UI) Debug.LogError($"{name} has no UI references");
        if (!previousButton) Debug.LogError($"{name} has no previous button references");
    }
    private void InitializeUI()
    {

    }
}