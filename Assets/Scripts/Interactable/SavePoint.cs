using Cysharp.Threading.Tasks;

public class SavePoint : InteractableTarget
{
    async protected override UniTask Interaction()
    {
        SaveLoadMenu.Instance.OpenPanel(null, null, true);
    }

    protected override UniTask PrintInteraction() => throw new System.NotImplementedException();

    protected override UniTask ScanInteraction() => throw new System.NotImplementedException();
}