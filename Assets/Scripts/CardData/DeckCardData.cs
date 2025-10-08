using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Deck Card Data")]
public class DeckCardData : CardData
{
    [SerializeField] protected List<CardOnDeckData> cardsOnDeck = new List<CardOnDeckData>();
    [SerializeField] protected bool isLimited;

    public List<CardOnDeckData> CardsOnDeck { get => cardsOnDeck; }
    public bool IsLimited { get => isLimited; }

    private void OnValidate()
    {
        cardType = CardType.Deck;
    }
}
