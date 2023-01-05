using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//#############%%#(////(%&#/*,,,,,,,,,,,,*/(%%%%##############################################%#(//(((////////////((###(/**,,,,
//############%%#(/////((#%#/,,,,,,,,,,,,,,,**(###############################################%%#(/((((((((((####(/***,,,,,,,,,
//###########%%#(/((((((((#%#/,,,,,,,,,,,,,,,,,**(#############################################%%#((((((((((/**,,,,,,,,,,,,,,,,
//##########%%%(((((((((((((##/*,,,,,,,,,,,,,,,,,,,*/##%########################################%%###((/**,,,,,,,,,,,,,,,,,,,,,
//#########%%%############(((###/*,,,,,,,,,,,,,,,,,,,,*/(##############################%%%%%%%###(/**,,,,,,,,,,,,,,,,,,,,,,,,,*
//#########%%#####################/*,,,,,,,,,,,,,,,,,,,,,**/(((((((((((/////**************,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,**(
//########%&%#####################%#/*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,**//((
//#######%&%######################%%%#/*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*/(####(
//######%&%#######%################%%%%#(*,,,,*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*/(########
//######%%%######%%%%%%%%%#%%%%%%%####%%%%#((/**,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,**/(###########
//#####%%%###%##%%%%%%%%%%%%%%%%%%%%%####%%#(*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,**////((##############
//####%&%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%(*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*/##################
//###%&%%%%%%%%%%%%%%%%%%%%%%%%&&&&&%%%%%%(*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,****,,,,,,,,,,,,,,,*/#################
//###&%%%%%%%%%%%%%%%%%%%%%%%%&&&&&%%##%%#/,,,,,,,**//(###(*,,,,,,,,,,,,,,,,,,,,,,,,,,*////#%%#/*,,,,,,,,,,,,*/################
//##%&%%#%%%%%%%%%%%%%%%%%%%%%%%%%####%%#/*,,,,,,,*((/*(%&%(/,,,,,,,,,,,,,,,,,,,,,,,,,*((**(%&%#/*,,,,,,,,,,,,*/#####((((((((((
//#%&&%%%%%%%%%%%%&&&%%%%%%%%#########%%#*,,,,,,,,*/#%%&&&&#/,,,,,,,,,,,,,,,,,,,,,,,,,*(%%%&&&%(/*,,,,,,,,,,,,,*(###(((((((((((
//%&&%%%%%%%%&&&&&&&&&&&&%%###########%%/,,,,,,,,,,,*/(###(/*,,,,,,,,,,,,,,,,,,,,,,,,,,**/((##(/,,,,,,,,,,,,,,,,*(##(((((((((((
//&&%%%%%%%&&&&&&&&&&&&&&%%%#########%%(*,,,,,,,,,,,,,,,,,,,,,,,,**/(#((/*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*/##(((((((((((
//&%###%%%%&&&&&&&&&&&&&&%%##########%%#(//***,,,,,,,,,,,,,,,,,,,,,*/(#((/,,,,,,,,,,,,,,,,,,,,,,,*,*******,,,,,,,*(#(((((((((((
//%#####%%%%%%%%%%%%%%%%%###########%&%%######(/*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*/(#((((((//*,,,,*/##((((((((((
//%##################((((((((((####%&%((((((((((*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*/(((((((((((/*,,,,*(##(((((((((
//%#####################((((######%&&%#(((((((#(*,,,,,,,,,,,,,,,,**//((((((/***,,,,,,,,,,,,,,,,/((((((((((#(/*,,,,,*(###(((((((
//%###############%%%%############%&%%###((((((*,,,,,,,,,,,,,,,,/(%%%#(/////(((/*,,,,,,,,,,,,,,*/(((((((((((/*,,,,,*/##########
//(########%%%%%%%%%%%%%%%#%%%%%#%%%(//(((((//*,,,,,,,,,,,,,,,,,/#%#(//******//(/*,,,,,,,,,,,,,,*/((((((((/*,,,,,,,,*/##((((((#
//(#############%%%#####%#%##%%%%%%%#/,,,,,,,,,,,,,,,,,,,,,,,,,,*(#(/********//(/*,,,,,,,,,,,,,,,,,,,,**,,,,,,,,,,,,,*(#(((((((
//#########%%%%#%%#%#####%#%%%%##%%%%#/,,,,,,,,,,,,,,,,,,,,,,,,,,*/(((///***/(((**,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,/##((((((
//(#####(((########((((((((((((((#(##%#/,,,,,,,,,,,,,,,,,,,,,,,,,,,,**//(((((//*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*(##((###
//#%%%%##((((((((((((((((((((((((((((###/*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*/#######
//%%%%%%#######((((((((((((((((((########/*,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*(######

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

    [Header("Code Machine: Sprites")]
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private Sprite m_onOpenSprite;
    [SerializeField] private Sprite m_onCloseSprite;

    [Header("Contain")]
    [SerializeField] private List<ContainReadonly> m_readonlyContains = new List<ContainReadonly>();
    [SerializeField] private List<ContainInputField> m_inputFieldContains = new List<ContainInputField>();

    [Header("Awake print")]
    [SerializeField] private string[] m_awakeString = null;

    [Header("On Broken")]
    [SerializeField] private bool m_broken = false;
    [SerializeField] private GameEvent[] m_onBrokeInterectEvent;

    [Header("On Infinite Loop")]
    [SerializeField] private GameEventWithOccurence m_onInfiniteLoopEvent;

    [Header("On Interact")]
    [SerializeField] private GameEvent[] m_onInteractEvent;

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

        UpdateSprite();

        if (!m_autoSave) return;

        if (m_autoSave is AutoSaveCodeMachine autoSave)
        {
            m_autoSaveCodeMachine = autoSave;
            m_autoSaveCodeMachine.AdditionalData(m_readonlyContains,m_inputFieldContains,m_compiler ? m_compiler.SaveData : new CompilerSaveData());
            m_autoSaveCodeMachine.AdditionalData2(m_onInfiniteLoopEvent.GetCurrentOccurenceNumber);
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

            m_broken = oldData.IsBroken;
            UpdateSprite();

            m_onCloseCodePossible = oldData.OnCloseCodePossible;
            if (oldData.PrintedMessages.Count > 0) PrintMessage(oldData.PrintedMessages.ToArray());

            for (int i = 0; i < m_readonlyContains.Count; i++) m_readonlyContains[i].LoadData(oldData.ReadOnlyContain[i]);
            for (int i = 0; i < m_inputFieldContains.Count; i++) m_inputFieldContains[i].LoadData(oldData.InputFieldContain[i]);
            if (m_compiler) m_compiler.LoadData(oldData.CompilerData);

            //Debug.Log($"{name}, load old save data!");
        }
    }
    #endregion

    private void OnMenuStateChanged(MenuState NewMenuState) => m_canClose = NewMenuState == MenuState.CodeMachineMK2;

    private void UpdateSprite()
    {
        if (!m_renderer) return;
        m_renderer.sprite = m_broken ? m_onCloseSprite : m_onOpenSprite; 
    }

    #region Interaction
    async protected override UniTask Interaction()
    {
        if (m_onInteractEvent.Length > 0) await GameManager.Instance.StartEvent(m_onInteractEvent);

        if (m_broken) GameManager.Instance.StartEvent(m_onBrokeInterectEvent).Forget();
        else OpenPanel(null);
    }
    async protected override UniTask PrintInteraction()
    {
        if (m_onInteractEvent.Length > 0) await GameManager.Instance.StartEvent(m_onInteractEvent);
        if (CannotPrintOrScan) return;

        List<string> InputData = new List<string>();
        foreach (ContainInputField contain in m_inputFieldContains) InputData.AddRange(contain.GetInputFieldText());
        PrintAndSaveMessage(m_compiler.PrintCompile(InputData.ToArray()));
    }
    async protected override UniTask ScanInteraction()
    {
        if (m_onInteractEvent.Length > 0) await GameManager.Instance.StartEvent(m_onInteractEvent);
        if (CannotPrintOrScan) return;

        m_scanOpen = true;
        OpenPanel(null);
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
        if (!m_popUpMessage || MessagesToPrint.Length <= 0) return;
        if (m_onInfiniteLoopEvent.GetEventLength > 0 && MessagesToPrint[0].Contains("INFINITE LOOP"))
        {
            GameManager.Instance.StartEvent(m_onInfiniteLoopEvent.Occur()).Forget();
            m_autoSaveCodeMachine.UpdateInfiniteLoopOccurenceNumber(m_onInfiniteLoopEvent.GetCurrentOccurenceNumber);
        }

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
    public void UpdateReadOnlyLine(ReadonlyUpdate Update)
    {
        if (Update == null || Update.IndexToUpdate < 0) return;
        if (m_readonlyContains.Count > Update.IndexToUpdate)
            m_readonlyContains[Update.IndexToUpdate].UpdateText(Update.UpdatedText);
    }
    public bool UnlockLine(string SearchedText, string ReplaceText, bool Fixed, bool InfiniteLoop)
    {
        foreach(ContainReadonly contain in m_readonlyContains)
        {
            if (contain.Text != SearchedText) continue;

            contain.UpdateText(ReplaceText);
            contain.Fixed(Fixed);
            m_compiler.UpdateInfiniteLoop(InfiniteLoop);

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
    public void UpdateBrokenState(bool IsBroken)
    {
        m_autoSaveCodeMachine.UpdateIsBroke(m_broken = IsBroken);
        UpdateSprite();
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
        if (m_scanPanel) m_scanPanel.SetActive(false);
        if (m_scanContainer) m_scanContainer.ResetInput();
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