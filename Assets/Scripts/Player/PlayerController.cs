using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Movement _movement;

    [Header("Input References")]
    [SerializeField] private InputActionReference _movementInput;
    [SerializeField] private InputActionReference _interactionInput;

    private bool canControl = false;

    #region Initialization
    private void Awake()
    {
        canControl = CheckNullReferences();
    }
    private bool CheckNullReferences()
    {
        bool checkResult = false;
        if (!_movement)
        {
            Debug.LogError($"{name} has no movement component for player controller");
            checkResult = true;
        }
        if (!_movementInput)
        {
            Debug.LogError($"{name} has no movement input references for player controller");
            checkResult = true;
        }
        return !checkResult;
    }
    #endregion

    private void FixedUpdate()
    {
        if (canControl)
        {
            Movement();
            Interaction();
        }
    }
    public void Movement()
    {
        Vector2 XY = _movementInput.action.ReadValue<Vector2>();
        
        if (XY.y > 0) _movement.MoveUp();
        else if (XY.y < 0) _movement.MoveDown();
        else if (XY.x > 0) _movement.MoveRight();
        else if (XY.x < 0) _movement.MoveLeft();
    }

    private void Interaction()
    {
        if (_interactionInput.action.ReadValue<float>() > 0) Debug.Log("Masuk");
    }
}
