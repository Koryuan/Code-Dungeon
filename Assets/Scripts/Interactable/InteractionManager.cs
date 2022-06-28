using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }
    private InteractableTarget focusedTarget;

    [SerializeField] private List<InteractableTarget> interactableTargets = new List<InteractableTarget>();

    private void Awake()
    {
        foreach(var target in interactableTargets)
        {
            target.onPlayerEnter += NewFocusTarget;
            target.onPlayerExit += UnFocusTarget;
        }
        Instance = this;
    }

    public void InteractTarget()
    {
        if (focusedTarget) focusedTarget.OnInteract();
    }

    public void NewFocusTarget(InteractableTarget Target)
    {
        focusedTarget = Target;
    }
    public void UnFocusTarget(InteractableTarget Target)
    {
        if (focusedTarget == Target) focusedTarget = null;
    }
}

public class InteractionEvent
{
    [SerializeField] private DialogSetting dialog;
    [SerializeField] private Item item;
}