using System;
using UnityEngine;

public abstract class InteractableTarget : MonoBehaviour 
{
    public event Action<InteractableTarget> onPlayerEnter;
    public event Action<InteractableTarget> onPlayerExit;

    public abstract void OnInteract();
    public abstract void OnUnInteract();

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision?.tag == "Player") onPlayerEnter?.Invoke(this);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision?.tag == "Player") onPlayerExit?.Invoke(this);
    }
}