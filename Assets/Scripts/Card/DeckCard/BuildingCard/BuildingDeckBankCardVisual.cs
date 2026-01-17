using UnityEngine;

public class BuildingDeckBankCardVisual : BuildingCardVisual
{
    BuildingDeckBankCardController deckBankCardController;

    protected override void Awake()
    {
        base.Awake();
        deckBankCardController = GetComponent<BuildingDeckBankCardController>();
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
