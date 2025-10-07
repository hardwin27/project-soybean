using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Deck Card Data")]
public class DeckCardData : CardData
{
    [SerializeField] protected List<CardOnDeckData> cardsOnDeck = new List<CardOnDeckData>();

    public List<CardOnDeckData> CardsOnDeck { get => cardsOnDeck; }

    private void OnValidate()
    {
        cardType = CardType.Deck;
    }
}
