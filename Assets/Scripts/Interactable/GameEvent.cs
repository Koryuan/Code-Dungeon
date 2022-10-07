using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class GameEvent
{
    [SerializeField] private DialogSetting dialog;
    [SerializeField] private Item item;
    [SerializeField] private GuideContent guide;
    [SerializeField] private UnityEvent eventAction;

    public bool HasEvent => eventAction != null;

    public DialogSetting Dialog => dialog;
    public GuideContent GuideContent => guide;
    public Item Item => item;
    public void ActiveAction() => eventAction?.Invoke();
}