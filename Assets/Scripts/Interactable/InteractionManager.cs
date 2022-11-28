using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }
    private InteractableTarget focusedTarget;

    [SerializeField] private PrintScanUI printScanSystem;

    private void Awake()
    {
        Instance = this;
    }

    #region Interaction
    public void InteractTarget()
    {
        if (focusedTarget) focusedTarget.OnInteract();
    }
    public void PrintInterectTarget()
    {
        if (focusedTarget && focusedTarget.CanPrintInterect) focusedTarget.OnPrintInteract();
    }
    public void ScanInterectTarget()
    {
        if (focusedTarget && focusedTarget.CanScanInterect) focusedTarget.OnScanInteract();
    }
    #endregion

    #region Update Target
    public void NewFocusTarget(InteractableTarget Target)
    {
        if (Target.CanInterect)
        {
            printScanSystem.OpenPrintScan(Target.CanPrintInterect,Target.CanScanInterect);
            focusedTarget = Target;
        }
    }
    public void UnFocusTarget(InteractableTarget Target)
    {
        if (focusedTarget == Target)
        {
            printScanSystem.ClosePrintScan();
            focusedTarget = null;
        }
    }
    #endregion
}