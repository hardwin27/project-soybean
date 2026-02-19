using System.Collections.Generic;
using UnityEngine;

public class BuildingDeckBuyCardController : BuildingDeckCardController
{
    protected ProgressionManager progressionManager;

    protected override void Awake()
    {
        base.Awake();

        progressionManager = ProgressionManager.Instance;
        if (progressionManager != null)
        {
            progressionManager.RegisterShop(this);
        }
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (CurrentCardOnDeck == null)
        {
            return;
        }

        if (CurrentCardOnDeck.CardData.BuyPrice <= 0)
        {
            OnDeckCardGenerated?.Invoke(CurrentCardOnDeck.CardData);

            ChangeToNextCard();
        }
    }

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

        AudioManager.Instance.PlaySFXObject("shop_on_purchase");

        int reqMoney = CurrentCardOnDeck.CardData.BuyPrice;

        /*for(int money = 0; money < reqMoney; money++) 
        {
            CardController card = cardStacks[cardStacks.Count - 1];
            cardStacks.Remove(card);
            card.gameObject.SetActive(false);
        }


        OnDeckCardGenerated?.Invoke(CurrentCardOnDeck.CardData);*/

        int money = 0;
        foreach (CardController card in cardStacks)
        {
            if (card.CardData is MoneyCardData moneyCardData)
            {
                money += moneyCardData.MoneyValue;
            }
            else
            {
                break;
            }
        }

        while (money > 0 && money >= reqMoney && cardStacks.Count > 0)
        {
            money -= reqMoney;
            CardController card = cardStacks[cardStacks.Count - 1];
            cardStacks.Remove(card);
            card.gameObject.SetActive(false);
            OnDeckCardGenerated?.Invoke(CurrentCardOnDeck.CardData);
        }

        ChangeToNextCard();
    }
}
