using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    #region Initialization
    private void Awake() => CheckNullReferences();
    private void CheckNullReferences()
    {
        if (!_animator) Debug.LogError($"{name} has no animator for animation");
    }
    #endregion

    public void UpdateMovemenetAnimation(float X, float Y)
    {
        _animator.SetFloat("X Coordinate", X);
        _animator.SetFloat("Y Coordinate", Y);
    }
}