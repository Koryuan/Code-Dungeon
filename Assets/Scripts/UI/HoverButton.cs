using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable] public class HoverButton : Button, IPointerEnterHandler, IPointerExitHandler
{
    public int buttonNumber = 0;
    public event Action<int> OnPointerHover;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (interactable) OnPointerHover?.Invoke(buttonNumber);
    }
}