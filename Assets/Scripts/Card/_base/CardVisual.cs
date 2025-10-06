using UnityEngine;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(CardController))]
public class CardVisual : MonoBehaviour
{
    [System.Serializable] 
    protected class CardTypeColor
    {
        public CardType CardType;
        public Color Color;
    }

    protected CardController cardController;

    [SerializeField] protected TextMeshPro cardNameText;
    [SerializeField] protected SpriteRenderer cardRenderer;
    [SerializeField] protected List<CardTypeColor> cardTypeColors;

    protected virtual void Awake()
    {
        cardController = GetComponent<CardController>();
        cardController.OnBaseDataUpdated += UpdateBaseVisual;
    }

    protected virtual void UpdateBaseVisual()
    {
        gameObject.name = cardController.CardData.CardName;
        cardNameText.text = cardController.CardData.CardName;
        foreach (var cardTypeColor in cardTypeColors)
        {
            if (cardTypeColor.CardType == cardController.CardData.CardType)
            {
                cardRenderer.color = cardTypeColor.Color;
                break;
            }
        }
    }
}
