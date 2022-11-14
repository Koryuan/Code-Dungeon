using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _TopTrigger;
    [SerializeField] private GameObject _BottomTrigger;
    [SerializeField] private GameObject _LeftTrigger;
    [SerializeField] private GameObject _RightTrigger;

    #region Initialization
    private void Awake() => CheckNullReferences();
    private void CheckNullReferences()
    {
        if (!_TopTrigger) Debug.LogError($"{name} has no Top Trigger for interaction");
        if (!_BottomTrigger) Debug.LogError($"{name} has no Bottom Trigger for interaction");
        if (!_LeftTrigger) Debug.LogError($"{name} has no Left Trigger for interaction");
        if (!_RightTrigger) Debug.LogError($"{name} has no Right Trigger for interaction");
    }
    #endregion

    #region Change Trigger
    public void UpdateRotation(Vector2 XY)
    {
        if (XY.y > 0) RotateUp();
        else if (XY.y < 0) RotateDown();
        else if (XY.x > 0) RotateRight();
        else if (XY.x < 0) RotateLeft();
    }
    public void RotateUp()
    {
        if (!_TopTrigger.activeSelf)
        {
            _TopTrigger.SetActive(true);
            _BottomTrigger.SetActive(false);
            _LeftTrigger.SetActive(false);
            _RightTrigger.SetActive(false);
        }
    }
    public void RotateDown()
    {
        if (!_BottomTrigger.activeSelf)
        {
            _TopTrigger.SetActive(false);
            _BottomTrigger.SetActive(true);
            _LeftTrigger.SetActive(false);
            _RightTrigger.SetActive(false);
        }
    }
    public void RotateLeft()
    {
        if (!_LeftTrigger.activeSelf)
        {
            _TopTrigger.SetActive(false);
            _BottomTrigger.SetActive(false);
            _LeftTrigger.SetActive(true);
            _RightTrigger.SetActive(false);
        }
    }
    public void RotateRight()
    {
        if (!_RightTrigger.activeSelf)
        {
            _TopTrigger.SetActive(false);
            _BottomTrigger.SetActive(false);
            _LeftTrigger.SetActive(false);
            _RightTrigger.SetActive(true);
        }
    }
    #endregion

    public void InteractTarget()
    {
        SaveLoadSystem.Instance._SaveData.PlayerLastPosition = transform.position;
        InteractionManager.Instance.InteractTarget();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractableTarget interactableTarget = collision.gameObject.GetComponent<InteractableTarget>();
        if (interactableTarget) InteractionManager.Instance.NewFocusTarget(interactableTarget);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        InteractableTarget interactableTarget = collision.gameObject.GetComponent<InteractableTarget>();
        if (interactableTarget) InteractionManager.Instance.UnFocusTarget(interactableTarget);
    }
}