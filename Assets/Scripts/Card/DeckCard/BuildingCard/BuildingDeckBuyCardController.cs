using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
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

        Debug.Log($"{gameObject.name} stacked money: {totalMoney} || CurrentCardOnDeck.CardData:  {CurrentCardOnDeck.CardData.BuyPrice}");

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

        int moneyRemoved = 0;

        Debug.Log($"{gameObject.name} Money: {money} || reqMoney: {reqMoney} cardStacks.Count: {cardStacks.Count}");

        while (money > 0 && money >= reqMoney && cardStacks.Count > 0)
        {
            while(moneyRemoved < reqMoney)
            {
                CardController card = cardStacks[cardStacks.Count - 1];
                cardStacks.Remove(card);
                card.gameObject.SetActive(false);
                money -= 1;
                moneyRemoved++;
            }

            OnDeckCardGenerated?.Invoke(CurrentCardOnDeck.CardData);
            moneyRemoved = 0;
        }

        ChangeToNextCard();
    }
}
