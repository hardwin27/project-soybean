using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReportProductEntryUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI productNameText;
    [SerializeField] private TextMeshProUGUI producedQtyText;
    [SerializeField] private Image producedArrowImage;
    [SerializeField] private Sprite increaseArrowSprite;
    [SerializeField] private Sprite decreaseArrowSprite;
    [SerializeField] private TextMeshProUGUI sellQtyText;

    public void DisplayProductEntryData(ReportProductEntryData reportProductEntryData)
    {
        productNameText.text = reportProductEntryData.ProductCardData.CardName;
        producedQtyText.text = reportProductEntryData.ProducedQty.ToString();
        if (reportProductEntryData.QuantityComparisonType == QuantityComparisonType.None)
        {
            producedArrowImage.gameObject.SetActive(false);
        }
        else
        {
            producedArrowImage.gameObject.SetActive(true);
            if (reportProductEntryData.QuantityComparisonType == QuantityComparisonType.Increased)
            {
                producedArrowImage.sprite = increaseArrowSprite;
            }
            else if (reportProductEntryData.QuantityComparisonType == QuantityComparisonType.Decreased)
            {
                producedArrowImage.sprite = decreaseArrowSprite;
            }
        }
        sellQtyText.text = (reportProductEntryData.SellQty == 0) ? $"-" : reportProductEntryData.SellQty.ToString();
    }
}
