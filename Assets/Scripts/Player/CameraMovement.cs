using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;

    [Header("References")]
    [SerializeField] private Transform playerPosition;

    [Header("Value")]
    [SerializeField] private float smoothness = 0.25f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0,0,0);

    #region Initialization
    private void Awake()
    {
        CheckNullReferences();
    }
    private void CheckNullReferences()
    {
        if (!playerPosition) Debug.LogError("No Player Poisition References");
    }
    #endregion

    private void LateUpdate() => MoveCamera();

    private void MoveCamera()
    {
        Vector3 targetPosition = playerPosition.position + cameraOffset;
        targetPosition.z = transform.position.z;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothness);
        transform.position = smoothedPosition;
    }
}