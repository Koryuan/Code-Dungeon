using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CodeMachine : InteractableTarget, IPanelUI
{
    [Header("Code Machine Properties")]
    [SerializeField] private GameObject panel;
    [SerializeField] private List<CodeMachineContain> m_containList = new List<CodeMachineContain>();

    private PrintFunction printFunction;
    private PopUpMessage printMessage;

    private bool canClose => panel.activeSelf && MenuManager.Instance.CodeMachineOpen;

    #region Initialization
    private void Awake()
    {
        CheckReferences();
        foreach(CodeMachineContain contain in m_containList) contain.Initialize();
        printFunction = GetComponent<PrintFunction>();
        printMessage = GetComponentInChildren<PopUpMessage>(true);
        Debug.Log(printMessage);
    }
    private void CheckReferences()
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
    async private void ClosePanel(InputAction.CallbackContext Context)
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
            if (contain is ReadonlyLine)
            {
                var line = contain as ReadonlyLine;
                if (line.BaseText == TextSearched)
                {
                    if (line.BaseText.Contains(StringList.PrintString))
                    {
                        printInteract = true;
                        InteractionManager.Instance.UpdateState();
                    }
                    line.UpdateText(line.BaseText.Replace("//", string.Empty));
                    return true;
                }
            }
        }
        return false;
    }

    #region Compiler
    private void CompileEachScript()
    {
        foreach(CodeMachineContain contain in m_containList)
        {
            string code = contain.BaseText;
            bool error = CompileOneLine(code);
            if (error) break;
        }
    }
    private bool CompileOneLine(string Code)
    {
        int count = 0;
        while (Code.Length > 0)
        {
            if (Code.Contains(StringList.Code_Print_Start))
            {
                string newCode = Code.Replace(StringList.Code_Print_Start, string.Empty);
                if (newCode.Contains(StringList.Code_Print_End))
                {
                    int index = newCode.IndexOf(StringList.Code_Print_End);
                    string tmpCode = newCode.Substring(0,index);
                    PrintMessage(tmpCode);
                    newCode = newCode.Replace(tmpCode,string.Empty);
                    newCode = newCode.Replace(StringList.Code_Print_End,string.Empty);
                    Debug.Log("This is runned");
                }
                else
                {
                    PrintMessage("Error");
                    return true;
                }
                Code = newCode;
            }
            #region Infinite LOOP Breaker
            count++;
            if (count > 100)
            {
                Debug.Log($"{count}, {Code}");
                PrintMessage("Infinite Loop");
                return true;
            }
            #endregion
        }
        return false;
    }
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
        CompileEachScript();
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
    async private void OnEnable()
    {
        await UniTask.WaitUntil(() => InputReferences.Instance);
        CloseInput.action.performed += ClosePanel;
    }
    private void OnDisable()
    {
        CloseInput.action.performed -= ClosePanel;
    }
    #endregion
}