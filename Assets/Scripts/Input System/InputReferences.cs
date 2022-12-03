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
    [SerializeField] private InputActionReference _menuCloseInput;

    public InputActionReference _PlayerMovementInput => _playerMovementInput;
    public InputActionReference _PlayerInteractionInput => _playerInteractionInput;
    public InputActionReference _PlayerInventoryInput => _playerInventoryInput;
    public InputActionReference _PlayerPauseInput => _playerPauseInput;
    public InputActionReference _MenuInterectInput => _menuInterectInput;
    public InputActionReference _MenuCloseInput => _menuCloseInput;

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