using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    public Image ArrowImage;
    public Sprite HighlightSprite;
    public Sprite NonHighlightSprite;
    public HoverButton Button;

    public void CheckReferences()
    {
        if (!ArrowImage) Debug.LogError($"{name} has no Arrow Image References");
        if (!HighlightSprite) Debug.LogError($"{name} has no Highlight sprite References");
        if (!NonHighlightSprite) Debug.LogError($"{name} has no Nonhighlight sprite References");
        if (!Button) Debug.LogError($"{name} has no Button References");
    }

    public void ChangeHighlight(bool IsHighlighted)
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