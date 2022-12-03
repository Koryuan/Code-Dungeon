﻿using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CodeMachine : InteractableTarget, IPanelUI
{
    [Header("Code Machine Properties")]
    [SerializeField] private GameObject panel;
    [SerializeField] private List<CodeMachineContain> m_containList = new List<CodeMachineContain>();

    private PrintFunction printFunction;

    private bool canClose => panel.activeSelf && MenuManager.Instance.CodeMachineOpen;

    #region Initialization
    private void Awake()
    {
        CheckReferences();
        foreach(CodeMachineContain contain in m_containList) contain.Initialize();
        printFunction = GetComponent<PrintFunction>();
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
                    if (line.BaseText.Contains(StringList.PrintCode))
                    {
                        line.UpdateText(line.BaseText.Replace("//",string.Empty));
                        printInteract = true;
                    } 
                    return true;
                }
            }
        }
        return false;
    }

    #region Interaction
    protected override UniTask Interaction()
    {
        var utcs = new UniTaskCompletionSource();

        OpenPanel(null);

        return utcs.Task;
    }

    async protected override UniTask PrintInteraction()
    {
        if (printFunction) await printFunction.Activate();
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