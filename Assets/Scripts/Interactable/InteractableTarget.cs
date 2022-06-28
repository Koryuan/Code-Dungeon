using System;
using UnityEngine;

public abstract class InteractableTarget : MonoBehaviour 
{
    public event Action<InteractableTarget> onPlayerEnter;
    public event Action<InteractableTarget> onPlayerExit;

    public abstract void OnInteract();
    public abstract void OnUnInteract();
}