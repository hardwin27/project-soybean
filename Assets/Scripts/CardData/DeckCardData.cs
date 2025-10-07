using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Deck Card Data")]
public class DeckCardData : CardData
{
    [SerializeField] protected List<CardData> containedCards = new List<CardData>();

    public List<CardData> ContainedCards { get => containedCards; }

    private void OnValidate()
    {
        cardType = CardType.Deck;
    }
}
