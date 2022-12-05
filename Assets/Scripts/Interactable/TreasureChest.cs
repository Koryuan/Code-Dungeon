using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class TreasureChest: InteractableTarget
{
    [Serializable] private struct TresureChestEvent
    {
        [SerializeField] private GameEvent[] onClose;
        [SerializeField] private GameEvent[] onOpen;

        public GameEvent[] GetEventList(bool Open) => Open ? onOpen : onClose;
    }

    [SerializeField] private bool isOpen = false;
    [SerializeField] private TresureChestEvent m_event;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;

    async protected override UniTask Interaction()
    {
        await GameManager.Instance.StartEvent(m_event.GetEventList(isOpen));
    }

    public void OpenTreasureChest(bool Open)
    {
        isOpen = Open;
        m_spriteRenderer.sprite = Open ? openSprite : closeSprite;

        // Interaction
        canInteract = Open;
        if (m_interactableAnimator) m_interactableAnimator.SetActive(true);
        AutoSaveScene.SaveObjectState($"{name} | Open");
    }
    public void ForceDisableInteraction() => DisableInteraction();
}