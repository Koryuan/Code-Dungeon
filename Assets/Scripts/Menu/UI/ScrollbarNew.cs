using UnityEngine;
using UnityEngine.UI;

public class ScrollbarNew : MonoBehaviour
{
    [SerializeField] private ScrollRect m_scrollRect;
    [SerializeField] private RectTransform m_scrollTransform;
    [SerializeField] private RectTransform m_contentTransform;
    [SerializeField] private RectTransform m_viewportTransform;

    private void Awake() => ResetViewport();

    public void CenterOnItem(RectTransform target)
    {
        // Item is here
        var itemCenterPositionInScroll = GetWorldPointInWidget(m_scrollTransform, GetWidgetWorldPoint(target));
        // But must be here
        var targetPositionInScroll = GetWorldPointInWidget(m_scrollTransform, GetWidgetWorldPoint(m_viewportTransform));
        // So it has to move this distance
        var difference = targetPositionInScroll - itemCenterPositionInScroll;
        difference.z = 0f;

        //clear axis data that is not enabled in the scrollrect
        if (!m_scrollRect.horizontal)
        {
            difference.x = 0f;
        }
        if (!m_scrollRect.vertical)
        {
            difference.y = 0f;
        }

        var normalizedDifference = new Vector2(
            difference.x / (m_contentTransform.rect.size.x - m_scrollTransform.rect.size.x),
            difference.y / (m_contentTransform.rect.size.y - m_scrollTransform.rect.size.y));

        var newNormalizedPosition = m_scrollRect.normalizedPosition - normalizedDifference;
        if (m_scrollRect.movementType != ScrollRect.MovementType.Unrestricted)
        {
            newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
            newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);
        }

        m_scrollRect.normalizedPosition = newNormalizedPosition;
    }
    private Vector3 GetWidgetWorldPoint(RectTransform target)
    {
        //pivot position + item size has to be included
        var pivotOffset = new Vector3(
            (0.5f - target.pivot.x) * target.rect.size.x,
            (0.5f - target.pivot.y) * target.rect.size.y,
            0f);
        var localPosition = target.localPosition + pivotOffset;
        return target.parent.TransformPoint(localPosition);
    }
    private Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
    {
        return target.InverseTransformPoint(worldPoint);
    }
    private void ResetViewport()
    {
        if (m_viewportTransform == null)
        {
            var mask = GetComponentInChildren<Mask>(true);
            if (mask)
            {
                m_viewportTransform = mask.rectTransform;
            }
            if (m_viewportTransform == null)
            {
                var mask2D = GetComponentInChildren<RectMask2D>(true);
                if (mask2D)
                {
                    m_viewportTransform = mask2D.rectTransform;
                }
            }
        }
    }
    #region Unused Scroll Function
    private bool m_instant = true;
    private float m_scrollSpeed = 0;
    private void UpdateScrollPosition(ItemBoxContain NewSelection)
    {
        // Math Calculation
        RectTransform selectedRectTransform = NewSelection.ContainerTransform;
        Vector3 selectedDifference = m_viewportTransform.localPosition - selectedRectTransform.localPosition;
        float contentHeightDifference = (m_contentTransform.rect.height - m_viewportTransform.rect.height);

        float selectedPosition = (m_contentTransform.rect.height - selectedDifference.y);
        float currentScrollRectPosition = m_scrollRect.normalizedPosition.y;
        float above = currentScrollRectPosition - (selectedRectTransform.rect.height / 2) + m_viewportTransform.rect.height;
        float below = currentScrollRectPosition + (selectedRectTransform.rect.height / 2);

        Debug.Log($"D: <color=red>{selectedDifference}</color>, CHD: <color=green>{contentHeightDifference}</color>, CSRP: <color=blue>{currentScrollRectPosition}</color>");
        Debug.Log($"SP: <color=red>{selectedPosition}</color>, A: <color=green>{above}</color>, B: <color=blue>{below}</color>");
        if (selectedPosition > above)
        {
            float step = selectedPosition - above;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            m_scrollRect.normalizedPosition = Vector2.Lerp(m_scrollRect.normalizedPosition, new Vector2(0, newNormalizedY), m_scrollSpeed * Time.deltaTime);
            if (m_instant) m_scrollRect.normalizedPosition = new Vector2(0, newNormalizedY);
        }
        else if (selectedPosition < below)
        {
            float step = selectedPosition - below;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            m_scrollRect.normalizedPosition = Vector2.Lerp(m_scrollRect.normalizedPosition, new Vector2(0, newNormalizedY), m_scrollSpeed * Time.deltaTime);
            if (m_instant) m_scrollRect.normalizedPosition = new Vector2(0, newNormalizedY);
        }
        Debug.Log($"Current Position: <color=green>{m_scrollRect.normalizedPosition}</color>");
    }
    #endregion
}