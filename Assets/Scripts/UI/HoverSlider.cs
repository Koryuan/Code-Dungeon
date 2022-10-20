using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable] public class HoverSlider : Slider, IPointerEnterHandler
{
    public event Action OnSelectHover;

    public override void OnSelect(BaseEventData eventData)
    {
        if (interactable) OnSelectHover?.Invoke();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (interactable) Select();
    }
}