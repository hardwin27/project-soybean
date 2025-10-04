using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CardVisual : MonoBehaviour
{
    [System.Serializable] 
    private class CardTypeColor
    {
        public CardType CardType;
        public Color Color;
    }

    [SerializeField] private TextMeshPro cardNameText;
    [SerializeField] private SpriteRenderer cardRenderer;
    [SerializeField] private List<CardTypeColor> cardTypeColors;

    public void SetVisual(CardData cardData)
    {
        gameObject.name = cardData.CardName;
        cardNameText.text = cardData.CardName;
        foreach (var cardTypeColor in  cardTypeColors) 
        {
            if (cardTypeColor.CardType == cardData.CardType)
            {
                cardRenderer.color = cardTypeColor.Color;
                break;
            }
        }
    }
}
