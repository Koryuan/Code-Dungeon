using Cysharp.Threading.Tasks;
using UnityEngine;

public class SavePoint : InteractableTarget
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    async public void Awake()
    {
        await UniTask.WaitUntil(() => SaveLoadMenu.Instance);
        SaveLoadMenu.Instance.OnClosePanel += () => UpdateSprite(false);
    }

    async protected override UniTask Interaction()
    {
        SaveLoadMenu.Instance.OpenPanel(null, null, true);
        UpdateSprite(true);
    }

    private void UpdateSprite(bool On) => _spriteRenderer.sprite = On? onSprite : offSprite;

    protected override UniTask PrintInteraction() => throw new System.NotImplementedException();

    protected override UniTask ScanInteraction() => throw new System.NotImplementedException();
}