using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerInteraction), typeof(PlayerAnimation))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Script References")]
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerInteraction _interaction;
    [SerializeField] private PlayerAnimation _animator;

    private bool canControl => GameManager.Instance.CurrentState == GameState.Game_Player_State;

    #region Initialization
    private void Awake() => CheckReferences();
    private void CheckReferences()
    {
        if (!_movement) Debug.LogError($"{name} has no movement script for player controller");
        if (!_interaction) Debug.LogError($"{name} has no interaction script for player controller");
        if (!_animator) Debug.LogError($"{name} has no animator script for player controller");
    }
    #endregion

    private void FixedUpdate()
    {
        Movement();
    }
    public void Movement()
    {
        Vector2 XY = canControl? movementInput.action.ReadValue<Vector2>() : Vector2.zero;

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

    #region Input References
    private InputActionReference movementInput => InputReferences.Instance._PlayerMovementInput;
    private InputActionReference interactionInput => InputReferences.Instance._PlayerInteractionInput;

    private void Interaction(InputAction.CallbackContext Callback)
    {
        if (canControl) _interaction.InteractTarget();
    }
    #endregion

    #region Enable Disable
    private void OnEnable()
    {
        interactionInput.action.performed += Interaction;
    }
    private void OnDisable()
    {
        interactionInput.action.performed -= Interaction;
    }
    #endregion
}
