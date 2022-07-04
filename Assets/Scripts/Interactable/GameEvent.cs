using System;
using UnityEngine;

[Serializable] public class GameEvent
{
    [SerializeField] private DialogSetting dialog;
    [SerializeField] private Item item;
    [SerializeField] private GuideContent guide;

    public DialogSetting Dialog => dialog;
    public GuideContent GuideContent => guide;
    public Item Item => item;
}