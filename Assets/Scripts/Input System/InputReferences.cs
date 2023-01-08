﻿using UnityEngine;
using UnityEngine.InputSystem;

public class InputReferences : MonoBehaviour
{
    public static InputReferences Instance;

    [Header("Player Input")]
    [SerializeField] private InputActionReference _playerMovementInput;
    [SerializeField] private InputActionReference _playerInteractionInput;
    [SerializeField] private InputActionReference _playerInventoryInput;
    [SerializeField] private InputActionReference _playerPauseInput;

    [Header("Menu Input")]
    [SerializeField] private InputActionReference _menuInterectInput;
    [SerializeField] private InputActionReference m_menu_Help;
    [SerializeField] private InputActionReference _menuCloseInput;

    [Header("Help Input")]
    [SerializeField] private InputActionReference m_help_LeftInput;
    [SerializeField] private InputActionReference m_help_RightInput;

    public InputActionReference _PlayerMovementInput => _playerMovementInput;
    public InputActionReference _PlayerInteractionInput => _playerInteractionInput;
    public InputActionReference _PlayerInventoryInput => _playerInventoryInput;
    public InputActionReference _PlayerPauseInput => _playerPauseInput;
    public InputActionReference _Menu_Interect => _menuInterectInput;
    public InputActionReference _Menu_Help => m_menu_Help;
    public InputActionReference _Help_Left => m_help_LeftInput;
    public InputActionReference _Help_Right => m_help_RightInput;
    public InputActionReference _Menu_Close => _menuCloseInput;

    private void Awake()
    {
        CheckReferences();
        Instance = this;
    }

    private void CheckReferences()
    {
        // Player
        if (!_playerMovementInput) Debug.LogError($"{name} has no movement input references for player");
        if (!_playerInteractionInput) Debug.LogError($"{name} has no interaction input references for player");
        if (!_playerInventoryInput) Debug.LogError($"{name} has no inventory input references for player");
        if (!_playerPauseInput) Debug.LogError($"{name} has no pause input references for player");

        // Menu
        if (!_menuInterectInput) Debug.LogError($"{name} has no interact input references for menu");
        if (!_menuCloseInput) Debug.LogError($"{name} has no close input references for menu");
    }
}