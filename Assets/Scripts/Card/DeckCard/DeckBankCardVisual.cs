using UnityEngine;

[RequireComponent (typeof(DeckBankCardController))]
public class DeckBankCardVisual : CardVisual
{
    DeckBankCardController deckBankCardController;

    protected override void Awake()
    {
        base.Awake();
        deckBankCardController = GetComponent<DeckBankCardController>();
        deckBankCardController.OnDeckBankDatUpdated += UpdateDeckBankVisual;
    }

    protected override void UpdateBaseVisual()
    {
        /*base.UpdateBaseVisual();*/
        UpdateBaseSprite(cardController.CardData.CardSprite, cardController.CardData.CardType);
        cardNameText.gameObject.SetActive(true);
    }

    protected void UpdateDeckBankVisual()
    {
        /*cardNameText.gameObject.SetActive(true);*/
        cardNameText.text = $"{deckBankCardController.CurrentMoney}/{deckBankCardController.BankDeckCardData.MaxMoney}";
    }
}
