using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class MaxLayoutElementSize : MonoBehaviour
{
    private RectTransform rectTransform;
    private LayoutElement layoutElement;

    public int maxLayoutWidth;
    public int maxLayoutHeight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        layoutElement = GetComponent<LayoutElement>();

        Update();
    }

    private void Update()
    {
        if (maxLayoutWidth > 0 && rectTransform.sizeDelta.x > maxLayoutWidth)
            layoutElement.preferredWidth = maxLayoutWidth;

        if (maxLayoutHeight > 0 && rectTransform.sizeDelta.y > maxLayoutHeight)
            layoutElement.preferredHeight = maxLayoutHeight;
    }
}
