using UnityEngine;

public class DebuggingTool : MonoBehaviour
{
    public static DebuggingTool Instance;

    [Header("Debugging")]
    [SerializeField] private bool debuging = false;
    [Range(30,60)][SerializeField] private int fpsLimit = 30;

    private void Awake()
    {
        Instance = debuging ? this : null;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fpsLimit;
    }
}