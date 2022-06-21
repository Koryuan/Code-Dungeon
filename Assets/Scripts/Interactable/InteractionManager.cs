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
            target.onPlayerEnter += FocusTarget;
            target.onPlayerExit += UnFocusTarget;
        }
    }

    private void FocusTarget(InteractableTarget Target)
    {
        focusedTarget = Target;
    }

    private void UnFocusTarget(InteractableTarget Target)
    {
        if (focusedTarget == Target) focusedTarget = null;
    }
}