using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[System.Serializable] public class PlayerMovement
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rigidbody;

    [Header("Movement value")]
    [SerializeField] private float movementSpeed;

    #region Initialization
    public void CheckReferences(string Name)
    {
        if (!_rigidbody) Debug.LogError($"{Name} has no rigidbody for movement");
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