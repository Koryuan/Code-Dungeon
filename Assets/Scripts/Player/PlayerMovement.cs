using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rigidbody;

    [Header("Movement value")]
    [SerializeField] private float movementSpeed;

    #region Initialization
    private void Awake() => CheckNullReferences();
    private void CheckNullReferences()
    {
        if (!_rigidbody) Debug.LogError($"{name} has no rigidbody for movement");
    }
    #endregion

    #region Movement
    public void MoveUp()
    {
        _rigidbody.MovePosition(_rigidbody.position + new Vector2(0,1) * movementSpeed * Time.fixedDeltaTime);
    }
    public void MoveDown()
    {
        _rigidbody.MovePosition(_rigidbody.position + new Vector2(0, -1) * movementSpeed * Time.fixedDeltaTime);
    }
    public void MoveRight()
    {
        _rigidbody.MovePosition(_rigidbody.position + new Vector2(1, 0) * movementSpeed * Time.fixedDeltaTime);
    }
    public void MoveLeft()
    {
        _rigidbody.MovePosition(_rigidbody.position + new Vector2(-1, 0) * movementSpeed * Time.fixedDeltaTime);
    }
    #endregion
}