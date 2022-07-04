using UnityEngine;

[CreateAssetMenu(menuName = "Guide", order = 101)]
public class GuideContent : ScriptableObject
{
    [SerializeField] private Sprite[] guideImage;

    public Sprite[] GuideImage => guideImage;
}