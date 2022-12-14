using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class GameEvent
{
    [SerializeField] protected DialogSetting dialog;
    [SerializeField] protected Item item;
    [SerializeField] protected HelpSettings m_help;
    [SerializeField] protected GuideContent guide;
    [SerializeField] protected UnityEvent eventAction;

    public bool HasEvent => eventAction != null;

    public DialogSetting Dialog => dialog;
    public GuideContent GuideContent => guide;
    public Item Item => item;
    public HelpSettings Help => m_help;
    public void ActiveAction() => eventAction?.Invoke();
    public void UpdateDialog(DialogSetting NewDialog, Item NewItem = null) => (dialog, item) = (NewDialog, NewItem); 
}