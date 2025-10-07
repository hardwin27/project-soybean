using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuyCardController : DeckCardController
{
    public override bool CanTakeCard(List<CardController> cardStacks)
    {
        int totalMoney = 0;
        foreach (CardController card in cardStacks) 
        {
            if (card.CardData is MoneyCardData moneyCardData) 
            {
                totalMoney += moneyCardData.MoneyValue;
            }
            else
            {
                return false;
            }
        }

        if (CurrentCardOnDeck == null) 
        {
            return false;
        }

        return (totalMoney >= CurrentCardOnDeck.CardData.BuyPrice);
    }

    public override void TakeCard(List<CardController> cardStacks)
    {
        base.TakeCard(cardStacks);

        int reqMoney = CurrentCardOnDeck.CardData.BuyPrice;
        
        for(int money = 0; money < reqMoney; money++) 
        {
            CardController card = cardStacks[cardStacks.Count - 1];
            cardStacks.Remove(card);
            card.gameObject.SetActive(false);
        }

        ChangeToNextCard();

        OnDeckCardGenerated?.Invoke(CurrentCardOnDeck.CardData);
    }
}
