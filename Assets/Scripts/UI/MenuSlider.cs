using UnityEngine;
using UnityEngine.UI;

public class MenuSlider : MonoBehaviour, IMenuUI
{
    public Image ArrowImage;
    public Sprite HighlightSprite;
    public Sprite NonHighlightSprite;
    public HoverSlider Slider;

    public void CheckReferences()
    {
        if (!ArrowImage) Debug.LogError($"{name} has no Arrow Image References");
        //if (!HighlightSprite) Debug.Log($"{name} has no Highlight sprite References");
        //if (!NonHighlightSprite) Debug.Log($"{name} has no Nonhighlight sprite References");
        if (!Slider) Debug.LogError($"{name} has no Slider References");
    }

    public void Select() => Slider.Select();

    public void SetHighlight(bool IsHighlighted)
    {
        if (IsHighlighted) ArrowImage.enabled = true;
        else ArrowImage.enabled = false;
    }
}