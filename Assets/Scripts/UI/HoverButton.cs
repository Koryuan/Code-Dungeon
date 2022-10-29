using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable] public class HoverButton : Button, IPointerEnterHandler, ISelectHandler
{
    public event Action OnSelectEvent;
    public event Action<bool> OnDeselectEvent;
    public event Action OnHoverEvent;

    public override void OnSelect(BaseEventData eventData)
    {
        if (interactable) OnSelectEvent?.Invoke();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (interactable) OnDeselectEvent?.Invoke(false);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (interactable)
        {
            OnHoverEvent?.Invoke();
            Select();
        }
    }
}