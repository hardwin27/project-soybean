using System.Collections.Generic;
using UnityEngine;

public class DeckSellCardController : DeckCardController
{
    [SerializeField] private MoneyCardData moneyCardData;
    
    public override bool CanTakeCard(List<CardController> cardStacks)
    {
        Debug.Log($"DECKSELL check can sell");

        foreach (CardController card in cardStacks) 
        { 
            if (card.CardData.SellPrice <= 0)
            {
                Debug.Log($"DECKSELL CANNOT SELL {card.CardData.CardName}");
                return false;
            }
        }

        return true;
    }

    public override void TakeCard(List<CardController> cardStacks)
    {
        int totalMoney = 0;

        Debug.Log($"DECKSELL stack length: {cardStacks.Count}");

        foreach(CardController card in cardStacks) 
        {
            totalMoney += card.CardData.SellPrice;
        }

        foreach (CardController card in cardStacks)
        {
            card.gameObject.SetActive(false);
        }

        for (int i = 0; i < totalMoney; i++)
        {
            OnDeckCardGenerated?.Invoke(moneyCardData);
        }
    }
}
