using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Threading;

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

    [SerializeField] protected bool isReqSpriteAssg = true;
    [SerializeField] protected TextMeshPro cardNameText;
    [SerializeField] protected SpriteRenderer cardRenderer;
    [SerializeField] protected Vector3 cardRendererSpriteScale;
    [SerializeField] protected List<CardTypeColor> cardTypeColors;
    /*[SerializeField] protected SimpleTooltip cardToolTip;*/

    protected virtual void Awake()
    {
        cardController = GetComponent<CardController>();
        cardController.OnBaseDataUpdated += UpdateBaseVisual;
    }

    protected virtual void UpdateBaseVisual()
    {
        CardData data = cardController.CardData;

        if (data != null)
        {
            gameObject.name = data.CardName;
            cardNameText.text = data.CardName;

            UpdateBaseSprite(data.CardSprite, data.CardType);   
        }
    }

    protected void DebugUpdateBaseSprite()
    {
        cardController = GetComponent<CardController>();
        if (cardController != null)
        {
            CardData cardData = cardController.CardData;
            if (cardData != null)
            {
                UpdateBaseSprite(cardData.CardSprite, cardData.CardType);
            }
        }
    }
    
    protected void UpdateBaseSprite(Sprite cardSprite, CardType cardType)
    {
        if (!isReqSpriteAssg)
        {
            return;
        }

        if (cardSprite != null)
        {
            cardRenderer.sprite = cardSprite;
            cardRenderer.transform.localScale = /*new Vector3(0.3f, 0.3f, 0.3f)*/cardRendererSpriteScale;
            cardNameText.gameObject.SetActive(false);
        }
        else
        {
            foreach (var cardTypeColor in cardTypeColors)
            {
                if (cardTypeColor.CardType == cardType)
                {
                    cardRenderer.color = cardTypeColor.Color;
                    break;
                }
            }
        }

        /*cardToolTip.iconSprite = cardSprite;*/
    }
}
