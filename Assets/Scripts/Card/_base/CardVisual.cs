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
        CardData data = cardController.CardData;

        gameObject.name = data.CardName;
        cardNameText.text = data.CardName;
        
        if (data.CardSprite != null)
        {
            cardRenderer.sprite = data.CardSprite;
            cardRenderer.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            cardNameText.gameObject.SetActive(false);
        }
        else
        {
            foreach (var cardTypeColor in cardTypeColors)
            {
                if (cardTypeColor.CardType == data.CardType)
                {
                    cardRenderer.color = cardTypeColor.Color;
                    break;
                }
            }
        }
    }
}
