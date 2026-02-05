using UnityEngine;
using SingletonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using ReadOnlyEditor;

/*[System.Serializable]
public class CardListing
{
    [ReadOnly] public CardData CardData;
    [ReadOnly] public int CardAmount;

    public CardListing(CardData _cardData)
    {
        CardData = _cardData;
        CardAmount = 0;
    }
}*/


public class CardGeneratorManager : Singleton<CardGeneratorManager>
{
    [SerializeField] private Collider2D boundaryCollider;
    [SerializeField, ReadOnly] private List<CardController> cards = new List<CardController>();

    public List<CardController> Cards { get =>  cards; }

    public Action<CardController> OnCardGenerated;

    private void Awake()
    {
        cards = FindObjectsByType<CardController>(FindObjectsSortMode.None).ToList();
        foreach (var card in cards)
        {
            Debug.Log($"CARD LOOP {card.CardData.CardName}");
            card.AssignBoundary(boundaryCollider);
        }
    }

    public CardController GenerateCard(CardData cardData, Vector3 pos)
    {
        GameObject generatedCardObj = null;

        generatedCardObj = Instantiate(cardData.CardPrefab);

        if (generatedCardObj != null)
        {
            generatedCardObj.transform.position = pos;
            if (generatedCardObj.TryGetComponent(out CardController cardController))
            {
                cards.Add(cardController);
                cardController.AssignCardData(cardData);
                cardController.AssignBoundary(boundaryCollider);
                OnCardGenerated?.Invoke(cardController);
                return cardController;
            }
        }

        return null;
    }

    /*private void AddToListing(CardData newCardData)
    {

    }*/

}


/*using UnityEngine;
using SingletonSystem;
using System;
using System.Collections.Generic;
using ReadOnlyEditor;

[System.Serializable]
public class CardListing
{
    [ReadOnly] public CardData CardData;
    [ReadOnly] public int CardAmount;

    public CardListing(CardData _cardData)
    {
        CardData = _cardData;
        CardAmount = 0;
    }
}

public class CardGeneratorManager : Singleton<CardGeneratorManager>
{
    [SerializeField] private GameObject BaseCardPrefab;
    [SerializeField] private GameObject ToolCardPrefab;

    [SerializeField, ReadOnly] private List<CardListing> cardListrings = new List<CardListing>();

    public Action<CardController> OnCardGenerated;

    public void GenerateCard(CardData cardData, Vector3 pos)
    {
        GameObject generatedCardObj = null;

        if (cardData.CardType == CardType.Tool)
        {
            generatedCardObj = Instantiate(ToolCardPrefab);
        }
        else
        {
            generatedCardObj = Instantiate(BaseCardPrefab);
        }

        if (generatedCardObj != null)
        {
            generatedCardObj.transform.position = pos;

            if (generatedCardObj.TryGetComponent(out CardController cardController))
            {
                cardController.AssignCardData(cardData);

                AddToListing(cardData);

                Action destructionHandler = null;
                destructionHandler = () =>
                {
                    RemoveFromListing(cardData);
                    cardController.OnCardDestroyed -= destructionHandler;
                };

                cardController.OnCardDestroyed += destructionHandler;

                OnCardGenerated?.Invoke(cardController);
            }
        }
    }

    private void AddToListing(CardData newCardData)
    {
        CardListing existingListing = cardListrings.Find(x => x.CardData == newCardData);

        if (existingListing != null)
        {
            existingListing.CardAmount++;
        }
        else
        {
            CardListing newListing = new CardListing(newCardData);
            newListing.CardAmount = 1;
            cardListrings.Add(newListing);
        }
    }

    private void RemoveFromListing(CardData cardDataToRemove)
    {
        CardListing existingListing = cardListrings.Find(x => x.CardData == cardDataToRemove);

        if (existingListing != null)
        {
            existingListing.CardAmount--;

            if (existingListing.CardAmount <= 0)
            {
                cardListrings.Remove(existingListing);
            }

        }
    }
}*/