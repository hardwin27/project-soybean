using TMPro;
using UnityEngine;

public class BuildingDeckBuyCardVisual : BuildingCardVisual
{
    protected BuildingDeckBuyCardController deckBuyCardController;
    [SerializeField] private TextMeshPro cardPriceText;

    protected override void Awake()
    {
        base.Awake();
        deckBuyCardController = GetComponent<BuildingDeckBuyCardController>();

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
            cardNameText.text = $"{deckBuyCardController.DeckCardData.InstructionCommandWord} {deckBuyCardController.CurrentCardOnDeck.CardData.CardName}";
            cardNameText.color = deckBuyCardController.DeckCardData.InstructionColor;
            cardPriceText.text = $"{deckBuyCardController.CurrentCardOnDeck.CardData.BuyPrice}";
            /*UpdateBaseSprite(deckBuyCardController.CurrentCardOnDeck.CardOnDeckSprite, deckBuyCardController.CurrentCardOnDeck.CardData.CardType);*/
        }
    }
}
