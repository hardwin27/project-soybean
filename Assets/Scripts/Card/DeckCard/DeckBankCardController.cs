using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckBankCardController : DeckCardController
{
    [SerializeField] protected BankDeckCardData bankDeckCardData;
    [SerializeField] protected int currentMoney;
    [SerializeField] private MoneyCardData moneyCardData;

    public Action OnDeckBankDatUpdated;

    public BankDeckCardData BankDeckCardData { get => bankDeckCardData; }
    public int CurrentMoney { get => currentMoney; }

    public override void AssignCardData(CardData data)
    {
        base.AssignCardData(data);
        bankDeckCardData = deckCardData as BankDeckCardData;

        OnDeckBankDatUpdated?.Invoke();
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        /*Debug.Log($"DECK BANK");*/

        if (currentMoney > 0)
        {
            currentMoney--;
            OnDeckCardGenerated?.Invoke(moneyCardData);
            OnDeckBankDatUpdated.Invoke();
        }

        AudioManager.Instance.PlaySFXObject("get_money");
    }

    public override bool CanTakeCard(List<CardController> cardStacks)
    {
        if (currentMoney >= bankDeckCardData.MaxMoney)
        {
            return false;
        }

        foreach (CardController card in cardStacks) 
        {
            if (card.CardData is not MoneyCardData moneyCardData) 
            {
                return false;
            }
        }

        return true;
    }

    public override void TakeCard(List<CardController> cardStacks)
    {
        base.TakeCard(cardStacks);

        while (cardStacks.Count > 0)
        {
            if (currentMoney >= bankDeckCardData.MaxMoney)
            {
                break;
            }

            CardController card = cardStacks[cardStacks.Count - 1];

            MoneyCardData moneyCardData = card.CardData as MoneyCardData;
            currentMoney += moneyCardData.MoneyValue;
            
            cardStacks.Remove(card);
            card.gameObject.SetActive(false);
        }

        OnDeckBankDatUpdated?.Invoke();

        AudioManager.Instance.PlaySFXObject("give_money");
    }
}
