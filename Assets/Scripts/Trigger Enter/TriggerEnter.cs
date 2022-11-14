using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerEnter : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.tag == "Player") OnPlayerEnter();
    }

    protected abstract void OnPlayerEnter();
}