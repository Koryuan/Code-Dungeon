using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject UIPanel;

    public bool isOpen => UIPanel.activeSelf;

    private void Awake()
    {
        CheckNullReferences();
    }

    private void CheckNullReferences()
    {
        if (!UIPanel) Debug.LogError("There is no UI Panel");
    }

    public void OpenClosePanel() => UIPanel.SetActive(!isOpen);
}