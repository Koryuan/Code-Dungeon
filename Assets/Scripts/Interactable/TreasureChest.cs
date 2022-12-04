using UnityEngine;

public class TreasureChest: InteractableObjectEvent
{
    [Header("Sprite")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;

    public void OpenTreasureChest(bool Open)
    {
        m_spriteRenderer.sprite = Open ? openSprite : closeSprite;

        // Interaction
        canInteract = Open;
        if (m_interactableAnimator) m_interactableAnimator.SetActive(true);
        AutoSaveScene.SaveObjectState($"{name} | Open");
    }
    public void ForceDisableInteraction() => DisableInteraction();
}