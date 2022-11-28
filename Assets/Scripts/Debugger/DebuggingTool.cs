using UnityEngine;

public class DebuggingTool : MonoBehaviour
{
    public static DebuggingTool Instance;

    [Header("Debugging")]
    [SerializeField] private bool debugging = false;
    [Range(30,60)][SerializeField] private int fpsLimit = 30;
    [SerializeField] private bool CloseMouse = false;

    private void Awake()
    {
        Instance = debugging ? this : null;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fpsLimit;
        if (CloseMouse)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}