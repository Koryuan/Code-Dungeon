using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IMenuUI
{
    public Image ArrowImage;
    public Sprite HighlightSprite;
    public Sprite NonHighlightSprite;
    public HoverButton Button;

    public void CheckReferences()
    {
        if (!ArrowImage) Debug.LogError($"{name} has no Arrow Image References");
        //if (!HighlightSprite) Debug.Log($"{name} has no Highlight sprite References");
        //if (!NonHighlightSprite) Debug.Log($"{name} has no Nonhighlight sprite References");
        if (!Button) Debug.LogError($"{name} has no Button References");
    }

    public void Initialize()
    {
        CheckReferences();
        Button.OnSelectEvent += () => SetHighlight(true);
        Button.OnDeselectEvent += SetHighlight;
    }

    public void Select() => Button.Select();

    public void SetHighlight(bool IsHighlighted)
    {
        if (IsHighlighted)
        {
            Button.image.sprite = HighlightSprite;
            ArrowImage.enabled = true;
        }
        else
        {
            Button.image.sprite = NonHighlightSprite;
            ArrowImage.enabled = false;
        }
    }
}