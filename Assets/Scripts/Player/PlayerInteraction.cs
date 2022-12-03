using UnityEngine;

[System.Serializable] public class PlayerInteraction
{
    [Header("References")]
    [SerializeField] private GameObject _TopTrigger;
    [SerializeField] private GameObject _BottomTrigger;
    [SerializeField] private GameObject _LeftTrigger;
    [SerializeField] private GameObject _RightTrigger;

    #region Initialization
    public void CheckReferences(string Name)
    {
        if (!_TopTrigger) Debug.LogError($"{Name} has no Top Trigger for interaction");
        if (!_BottomTrigger) Debug.LogError($"{Name} has no Bottom Trigger for interaction");
        if (!_LeftTrigger) Debug.LogError($"{Name} has no Left Trigger for interaction");
        if (!_RightTrigger) Debug.LogError($"{Name} has no Right Trigger for interaction");
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

    public void InteractTarget(Vector3 Position)
    {
        SaveLoadSystem.Instance._SaveData.PlayerLastPosition = Position;
        InteractionManager.Instance.InteractTarget();
    }

    public void TargetEnter(Collider2D collision)
    {
        
        InteractableTarget interactableTarget = collision.gameObject.GetComponent<InteractableTarget>();
        if (interactableTarget) InteractionManager.Instance.NewFocusTarget(interactableTarget);
    }
    public void TargetExit(Collider2D collision)
    {
        InteractableTarget interactableTarget = collision.gameObject.GetComponent<InteractableTarget>();
        if (interactableTarget) InteractionManager.Instance.UnFocusTarget(interactableTarget);
    }
}