using System;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite_Renderer;

    public event Action<string> OnFunctionActivated;

    public void ChangeColorRed()
    {
        sprite_Renderer.color = Color.red;
        OnFunctionActivated?.Invoke("Change color red");
    }

    public void ChangeColorBlue()
    {
        sprite_Renderer.color = Color.blue;
        OnFunctionActivated?.Invoke("Change color blue");
    }

    public void ChangeColorGreen()
    {
        sprite_Renderer.color = Color.green;
        OnFunctionActivated?.Invoke("Change color green");
    }

    public void ChangeColorWhite()
    {
        sprite_Renderer.color = Color.white;
        OnFunctionActivated?.Invoke("Change color white");
    }
}
