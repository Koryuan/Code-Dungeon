using Cysharp.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerInteraction _interaction;
    [SerializeField] private PlayerAnimation _animator;
    [SerializeField] private PlayerCamera _camera;
    [SerializeField] private PlayerSound _sound;

    private bool canControl => GameManager.Instance.CurrentState == GameState.Game_Player_State;

    #region Initialization
    private void Awake() => CheckReferences();
    private void CheckReferences()
    {
        _animator.CheckReferences(name);
        _interaction.CheckReferences(name);
        _movement.CheckReferences(name);
        _camera.CheckReference();
    }
    #endregion

    private void FixedUpdate()
    {
        Movement();
    }

    public UniTask MoveCamera(bool NormalPosition) => _camera.MoveCamera(NormalPosition);

    #region Movement
    private void Movement()
    {
        Vector2 XY = canControl? movementInput.action.ReadValue<Vector2>() : Vector2.zero;
        if (XY.x != 0 || XY.y != 0) _sound.PlayWalkSound();
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
        else
        {
            _animator.UpdateMovemenetAnimation(0, 0);
            _sound.StopWalkSounds();
        }
    }
    public void InstantMove(Transform NewPosition, Vector2 Rotation)
    {
        transform.position = NewPosition.position;
        _interaction.UpdateRotation(Rotation);
        _animator.UpdateMovemenetAnimation(Rotation.x, Rotation.y);
    }
    public void InstantMove(Vector3 NewPosition)
    {
        transform.position = NewPosition;
    }
    #endregion

    #region Input References
    private InputActionReference movementInput => InputReferences.Instance._PlayerMovementInput;
    private InputActionReference interactionInput => InputReferences.Instance._PlayerInteractionInput;

    private void Interaction(InputAction.CallbackContext Callback)
    {
        if (canControl) _interaction.InteractTarget(transform.position);
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision) => _interaction.TargetEnter(collision);

    private void OnTriggerExit2D(Collider2D collision) => _interaction.TargetExit(collision);
    #endregion

    #region Enable Disable
    private async void OnEnable()
    {
        await UniTask.WaitUntil(() => InputReferences.Instance != null);
        interactionInput.action.performed += Interaction;
    }
    private void OnDisable()
    {
        interactionInput.action.performed -= Interaction;
    }
    #endregion
}
