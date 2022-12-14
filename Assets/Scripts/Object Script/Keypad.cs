using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Keypad : InteractableTarget
{
    [Header("Keypad Attribute")]
    [SerializeField] private string m_text;

    [Header("Sprite References")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Sprite m_interactedSprite;
    [SerializeField] private Sprite m_unInteractedSprite;

    public bool UseSprite => m_spriteRenderer && m_interactedSprite && m_unInteractedSprite;
    public string Text => m_text;

    public Action<string> OnInteracted;

    protected override UniTask Interaction()
    {
        var utcs = new UniTaskCompletionSource();

        OnInteracted?.Invoke(m_text);
        UpdateLook(true);

        return utcs.Task;
    }

    public void UpdateLook(bool On)
    {
        if (UseSprite) m_spriteRenderer.sprite = On ? m_interactedSprite : m_unInteractedSprite;
        canInteract = !On;
    }
    public void UpdateInteraction(bool On) => canInteract = On;
}