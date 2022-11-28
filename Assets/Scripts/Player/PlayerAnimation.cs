using UnityEngine;

[System.Serializable] public class PlayerAnimation
{
    [SerializeField] private Animator _animator;

    #region Initialization
    public void CheckReferences(string Name)
    {
        if (!_animator) Debug.LogError($"{Name} has no animator for animation");
    }
    #endregion

    public void UpdateMovemenetAnimation(float X, float Y)
    {
        _animator.SetFloat("X Coordinate", X);
        _animator.SetFloat("Y Coordinate", Y);
    }

    public Vector2 GetXYCoordinate() => new Vector2(_animator.GetFloat("X Coordinate"), _animator.GetFloat("Y Coordinate"));
}