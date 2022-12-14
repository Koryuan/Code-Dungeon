using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CodeMachineMK2 : InteractableTarget, IPanelUI
{
    [Header("Code Machine Properties")]
    [SerializeField, Tooltip("This one is for code panel")] private GameObject m_codePanel;
    [SerializeField, Tooltip("This one is for scan panel")] private GameObject m_scanPanel;

    [Header("Code Machine: References")]
    [SerializeField, Tooltip("This one is menu channel")] private MenuManager m_menuManager;
    [SerializeField, Tooltip("This one is to pop out some messsage")] private CodeMachineMK2Message m_popUpMessage;
    [SerializeField, Tooltip("Use to compile the code and give string to print")] private CodeCompiler m_compiler;
    [SerializeField, Tooltip("Container for Scan")] private CodeMachineScan m_scanContainer;

    [Header("Contain")]
    [SerializeField] private List<ContainReadonly> m_readonlyContains = new List<ContainReadonly>();
    [SerializeField] private List<ContainInputField> m_inputFieldContains = new List<ContainInputField>();

    [Header("Awake print")]
    [SerializeField] private string[] m_awakeString = null;

    [Header("On Close Code")]
    [SerializeField] private bool m_onCloseCodeOnce = false;
    [SerializeField] private GameEvent[] m_onCloseCodeEvent;

    private bool m_onCloseCodePossible { get; set; } = true;
    private bool m_canClose { get; set; }
    private bool m_scanOpen { get; set; } = false;
    private AutoSaveCodeMachine m_autoSaveCodeMachine { get; set; } = null;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        CheckReferences();

        if (m_menuManager) m_menuManager.OnMenuStateChanged += OnMenuStateChanged;
        if (m_awakeString != null) PrintMessage(m_awakeString);

        // Initialize contain and compiler
        for (int i = 0; i < m_readonlyContains.Count; i++)
        {
            m_readonlyContains[i].Initialize(i);
            if (m_autoSaveCodeMachine) m_readonlyContains[i].OnUpdate += m_autoSaveCodeMachine.UpdateReadOnlyContain;
        }
        for (int i = 0; i < m_inputFieldContains.Count; i++)
        {
            m_inputFieldContains[i].Initailize(i);
            if (m_autoSaveCodeMachine) m_inputFieldContains[i].OnUpdate += m_autoSaveCodeMachine.UpdateInputFieldContain;
        }
        if (m_compiler)
        {
            m_compiler.Initialize();
            if (m_autoSaveCodeMachine) m_compiler.OnUpdate += m_autoSaveCodeMachine.UpdateCompilerOccurence;
        }
        if (m_scanContainer)
        {
            m_scanContainer.Initialize();
            m_scanContainer.OnInputButtonClicked += ScanInputReceive;
        }

        if (!m_autoSave) return;

        if (m_autoSave is AutoSaveCodeMachine autoSave)
        {
            m_autoSaveCodeMachine = autoSave;
            m_autoSaveCodeMachine.AdditionalData(m_readonlyContains,m_inputFieldContains,m_compiler ? m_compiler.SaveData : new CompilerSaveData());
            m_autoSave.OnDataLoaded += LoadData;
            m_autoSave.LoadData(canInteract, printInteract, scanInteract, gameObject.activeSelf
                , m_interactableAnimator ? m_interactableAnimator.activeSelf : false);
        }
    }
    private void CheckReferences()
    {
        if (!m_codePanel) Debug.LogError($"{name}, has no panel to open");
        if (!m_compiler) Debug.LogWarning($"{name}, has no compiler to open");
    }
    private void LoadData(SaveDataAuto LoadedData)
    {
        if (LoadedData.New) return;
        if (LoadedData is CodeMachineSaveData oldData)
        {
            canInteract = oldData.CanInteract;
            printInteract = oldData.CanPrint;
            scanInteract = oldData.CanScan;
            gameObject.SetActive(oldData.ObjectActive);
            if (m_interactableAnimator) m_interactableAnimator.SetActive(oldData.AnimationActive);

            m_onCloseCodePossible = oldData.OnCloseCodePossible;
            if (oldData.PrintedMessages.Count > 0) PrintMessage(oldData.PrintedMessages.ToArray());

            for (int i = 0; i < m_readonlyContains.Count; i++) m_readonlyContains[i].LoadData(oldData.ReadOnlyContain[i]);
            for (int i = 0; i < m_inputFieldContains.Count; i++) m_inputFieldContains[i].LoadData(oldData.InputFieldContain[i]);
            if (m_compiler) m_compiler.LoadData(oldData.CompilerData);

            Debug.Log($"{name}, load old save data!");
        }
    }
    #endregion

    private void OnMenuStateChanged(MenuState NewMenuState) => m_canClose = NewMenuState == MenuState.CodeMachineMK2;

    #region Interaction
    protected override UniTask Interaction()
    {
        var utcs = new UniTaskCompletionSource();

        OpenPanel(null);

        return utcs.Task;
    }
    protected override UniTask PrintInteraction()
    {
        var utcs = new UniTaskCompletionSource();

        if (CannotPrintOrScan) return utcs.Task;
            
        List<string> InputData = new List<string>();
        foreach (ContainInputField contain in m_inputFieldContains) InputData.AddRange(contain.GetInputFieldText());
        PrintAndSaveMessage(m_compiler.PrintCompile(InputData.ToArray()));
        return utcs.Task;
    }
    protected override UniTask ScanInteraction()
    {
        var utcs = new UniTaskCompletionSource();

        if (CannotPrintOrScan) return utcs.Task;

        m_scanOpen = true;
        OpenPanel(null);

        return utcs.Task;
    }
    private void ScanInputReceive(string[] ScanInputs)
    {
        ClosePanel(new InputAction.CallbackContext());
        List<string> InputData = new List<string>();
        foreach (ContainInputField contain in m_inputFieldContains) InputData.AddRange(contain.GetInputFieldText());
        PrintAndSaveMessage(m_compiler.ScanCompile(InputData.ToArray(),ScanInputs));
    }
    private bool CannotPrintOrScan => CheckReadOnlyERROR() || CheckCompiler();
    private bool CheckReadOnlyERROR()
    {
        foreach (ContainReadonly contain in m_readonlyContains)
        {
            if (contain.Error)
            {
                PrintAndSaveMessage(new[]
                {
                    StringList.ColorString("IMPORTANT LINE",StringList.Color_Red),
                    StringList.ColorString("IS",StringList.Color_Red),
                    StringList.ColorString("NOT UNLOCK",StringList.Color_Red)
                });
                return true;
            }
        }
        return false;
    }
    private bool CheckCompiler()
    {
        if (!m_compiler)
        {
            PrintAndSaveMessage(new[] { StringList.ColorString("ERROR", StringList.Color_Red), StringList.ColorString("NO COMPILER", StringList.Color_Red) });
            return true;
        }
        return false;
    }
    #endregion

    #region Print/Scan Function
    public void PrintMessage(string[] MessagesToPrint)
    {
        if (!m_popUpMessage) return;
        if (MessagesToPrint.Length == 1) m_popUpMessage.PrintOnly1Message(MessagesToPrint[0]);
        else m_popUpMessage.PrintMessage(MessagesToPrint);
    }
    public void PrintAndSaveMessage(string[] MessagesToPrint)
    {
        PrintMessage(MessagesToPrint);
        if (m_autoSaveCodeMachine) m_autoSaveCodeMachine.UpdateOnMessagePrint(MessagesToPrint);
    }
    #endregion

    #region Unlock
    public bool UnlockLine(string SearchedText, string ReplaceText, bool Fixed)
    {
        foreach(ContainReadonly contain in m_readonlyContains)
        {
            if (contain.Text != SearchedText) continue;

            contain.UpdateText(ReplaceText);
            contain.Fixed(Fixed);

            if (ReplaceText.Contains(StringList.Pseudocode_Scan))
            {
                ScanActivation(true);
                PrintActivation(false);
            }
            if (ReplaceText.Contains(StringList.Pseudocode_Print) && !scanInteract) PrintActivation(true);

            return true;
        }
        return false;
    }
    #endregion

    #region Open/Close
    InputActionReference closeInput => InputReferences.Instance._Menu_Close;
    async public void OpenPanel(IMenuUI LastUI)
    {
        if (m_menuManager) m_menuManager.OpenMenu(this, null);
        await GameManager.Instance.Player.MoveCamera(true);
        m_codePanel.SetActive(!m_scanOpen);
        m_scanPanel.SetActive(m_scanOpen);
    }
    async private void ClosePanel(InputAction.CallbackContext Context)
    {
        if (!m_canClose || !(m_codePanel.activeSelf || m_scanPanel.activeSelf)) return;

        bool fromCode = m_codePanel.activeSelf;

        m_codePanel.SetActive(false);
        m_scanPanel.SetActive(false);
        m_scanOpen = false;

        await GameManager.Instance.Player.MoveCamera(false);
        if (m_menuManager) m_menuManager.CloseMenu(this);

        if (!m_onCloseCodePossible || m_onCloseCodeEvent.Length == 0 || !fromCode) return;

        await GameManager.Instance.StartEvent(m_onCloseCodeEvent);
        if (!m_onCloseCodeOnce) return;

        m_onCloseCodePossible = false;
        m_autoSaveCodeMachine.UpdateOnClosePossible(m_onCloseCodePossible);
    }
    #endregion

    #region Enable/Disable/Destroy
    async private void OnEnable()
    {
        await UniTask.WaitUntil(() => InputReferences.Instance);
        closeInput.action.performed += ClosePanel;
    }
    private void OnDisable()
    {
        closeInput.action.performed -= ClosePanel;
    }
    private void OnDestroy()
    {
        if (m_menuManager) m_menuManager.OnMenuStateChanged -= OnMenuStateChanged;
        if (m_autoSaveCodeMachine) m_autoSave.OnDataLoaded -= LoadData;
    }
    #endregion
}