using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerInteraction), typeof(PlayerAnimation))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Script References")]
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerInteraction _interaction;
    [SerializeField] private PlayerAnimation _animator;

    [Header("Input References")]
    [SerializeField] private InputActionReference _movementInput;
    [SerializeField] private InputActionReference _interactionInput;

    private bool canControl => GameManager.Instance.CurrentState == GameState.PlayerControl;

    #region Initialization
    private void Awake() => CheckNullReferences();
    private void CheckNullReferences()
    {
        if (!_movement) Debug.LogError($"{name} has no movement script for player controller");
        if (!_movementInput) Debug.LogError($"{name} has no movement input references for player controller");
        if (!_interaction) Debug.LogError($"{name} has no interaction script for player controller");
        if (!_interactionInput) Debug.LogError($"{name} has no intertaction input references for player controller");
    }
    #endregion

    private void FixedUpdate()
    {
        if (canControl)
        {
            Movement();
        }
    }
    public void Movement()
    {
        Vector2 XY = _movementInput.action.ReadValue<Vector2>();

        if (XY.y > 0)
        {
            _movement.MoveUp();
            _interaction.RotateUp();
            _animator.UpdateMovemenetAnimation(0, XY.y);
        }
        else if (XY.y < 0)
        {
            _movement.MoveDown();
            _interaction.RotateDown();
            _animator.UpdateMovemenetAnimation(0, XY.y);
        }
        else if (XY.x > 0)
        {
            _movement.MoveRight();
            _interaction.RotateRight();
            _animator.UpdateMovemenetAnimation(XY.x, 0);
        }
        else if (XY.x < 0)
        {
            _movement.MoveLeft();
            _interaction.RotateLeft();
            _animator.UpdateMovemenetAnimation(XY.x, 0);
        }
        else _animator.UpdateMovemenetAnimation(0,0);
    }
    private void Interaction(InputAction.CallbackContext Callback)
    {
        if (canControl) _interaction.InteractTarget();
    }

    #region Enable Disable
    private void OnEnable()
    {
        InitializePlayerInput();
    }
    private void OnDisable()
    {
        UnRefPlayerInput();
    }
    private void InitializePlayerInput()
    {
        _interactionInput.action.performed += Interaction;
    }
    private void UnRefPlayerInput()
    {
        _interactionInput.action.performed -= Interaction;
    }
    #endregion
}
