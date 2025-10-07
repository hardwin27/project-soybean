using TMPro;
using UnityEngine;

[RequireComponent (typeof(DeckBuyCardController))]
public class DeckBuyCardVisual : CardVisual
{
    protected DeckBuyCardController deckBuyCardController;
    [SerializeField] private TextMeshPro cardPriceText;

    protected override void Awake()
    {
        base.Awake();
        deckBuyCardController = GetComponent<DeckBuyCardController>();

        deckBuyCardController.OnDeckCardUpdated += UpdateDeckBuyVisual;
        /*deckBuyCardController.OnDeckCardGenerated += (card) =>
        {
            UpdateDeckBuyVisual();
        };*/
    }

    protected override void UpdateBaseVisual()
    {
        UpdateBaseSprite(cardController.CardData.CardSprite, cardController.CardData.CardType);
        cardNameText.gameObject.SetActive(true);
        return;
    }

    protected void UpdateDeckBuyVisual()
    {
        CardOnDeckData cardOnDeck = deckBuyCardController.CurrentCardOnDeck;
        if (cardOnDeck == null)
        {
            cardNameText.text = deckBuyCardController.DeckCardData.CardName;
            cardPriceText.text = "";
            /*UpdateBaseSprite(null, deckBuyCardController.CardData.CardType);*/
        }
        else
        {
            cardNameText.text = $"Buy {deckBuyCardController.CurrentCardOnDeck.CardData.CardName}";
            cardPriceText.text = $"{deckBuyCardController.CurrentCardOnDeck.CardData.BuyPrice}";
            /*UpdateBaseSprite(deckBuyCardController.CurrentCardOnDeck.CardOnDeckSprite, deckBuyCardController.CurrentCardOnDeck.CardData.CardType);*/
        }
    }
}
