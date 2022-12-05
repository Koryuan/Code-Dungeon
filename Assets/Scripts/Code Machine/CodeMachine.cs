using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CodeMachine : InteractableTarget, IPanelUI
{
    [Header("Code Machine Properties")]
    [SerializeField] protected GameObject panel;
    [SerializeField] protected List<CodeMachineContain> m_containList = new List<CodeMachineContain>();

    protected PrintFunction printFunction;
    protected PopUpMessage printMessage;

    protected bool canClose => panel.activeSelf && MenuManager.Instance.CodeMachineOpen;

    #region Initialization
    protected virtual void Awake()
    {
        CheckReferences();
        foreach(CodeMachineContain contain in m_containList) contain.Initialize();
        printFunction = GetComponent<PrintFunction>();
        printMessage = GetComponentInChildren<PopUpMessage>(true);
        Debug.Log(printMessage);
    }
    protected void CheckReferences()
    {
        if (!panel) Debug.LogError($"{name}, has no panel to open");
    }
    #endregion

    #region Open Close
    InputActionReference CloseInput => InputReferences.Instance._MenuCloseInput;
    async public void OpenPanel(IMenuUI LastUI)
    {
        MenuManager.Instance.OpenMenu(this);
        await GameManager.Instance.Player.MoveCamera(true);
        panel.SetActive(true);
    }
    async protected void ClosePanel(InputAction.CallbackContext Context)
    {
        if (canClose)
        {
            panel.SetActive(false);
            await GameManager.Instance.Player.MoveCamera(false);
            MenuManager.Instance.CloseMenu(this);
        }
    }
    #endregion
     
    public bool UnlockText(string TextSearched)
    {
        foreach(CodeMachineContain contain in m_containList)
        {
            if (contain is LineReadonly)
            {
                var line = contain as LineReadonly;
                if (line.BaseText == TextSearched)
                {
                    if (line.BaseText.Contains(StringList.PrintString))
                    {
                        printInteract = true;
                        InteractionManager.Instance.UpdateState();
                    }
                    line.UpdateText(line.BaseText.Replace("//", string.Empty));
                    Debug.Log("Text Finded");
                    return true;
                }
                Debug.Log($"Text Didn't Find: {line.BaseText} != {TextSearched}");
            }
        }
        return false;
    }

    #region Compiler
    public void PrintMessage(string TextToPrint)
    {
        if (printMessage) printMessage.OpenMessage(TextToPrint);
        else Debug.Log($"{name} Print Message Not Exist");
    }
    #endregion

    #region Interaction
    protected override UniTask Interaction()
    {
        var utcs = new UniTaskCompletionSource();

        OpenPanel(null);

        return utcs.Task;
    }

    async protected override UniTask PrintInteraction()
    {
        if (printFunction)
        {
            await printFunction.Activate();
            AutoSaveScene.SaveObjectState($"{name} | Print");
            Debug.Log($"{name} | Print");
        }
        else Debug.LogError($"{name}, trying to use function from print without having the class");
    }

    protected override UniTask ScanInteraction()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region Enable/Disable
    async protected void OnEnable()
    {
        await UniTask.WaitUntil(() => InputReferences.Instance);
        CloseInput.action.performed += ClosePanel;
    }
    protected void OnDisable()
    {
        CloseInput.action.performed -= ClosePanel;
    }
    #endregion
}