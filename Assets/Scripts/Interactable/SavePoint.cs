using Cysharp.Threading.Tasks;
using System;
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

    protected override UniTask Interaction()
    {
        var utcs = new UniTaskCompletionSource();

        SaveLoadMenu.Instance.OpenPanel(null, null, true);
        UpdateSprite(true);

        return utcs.Task;
    }

    private void UpdateSprite(bool On) => _spriteRenderer.sprite = On? onSprite : offSprite;

    protected override UniTask PrintInteraction() => throw new NotImplementedException();

    protected override UniTask ScanInteraction() => throw new NotImplementedException();
}