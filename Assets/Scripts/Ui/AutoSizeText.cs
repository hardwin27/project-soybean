using UnityEngine;
using TMPro;

public class AutoSizeText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpText;
    [SerializeField] private RectTransform rectTransform;

    public void UpdateHeight()
    {
        tmpText.ForceMeshUpdate();

        float width = rectTransform.rect.width;

        Vector2 preferredSize = tmpText.GetPreferredValues(tmpText.text, width, float.PositiveInfinity);

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, preferredSize.y);
    }
}