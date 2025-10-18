using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Deck Card Data")]
public class DeckCardData : CardData
{
    [SerializeField] protected List<CardOnDeckData> cardsOnDeck = new List<CardOnDeckData>();
    [SerializeField] protected bool isLimited;
    [SerializeField] protected Color instructionColor;

    public List<CardOnDeckData> CardsOnDeck { get => cardsOnDeck; }
    public bool IsLimited { get => isLimited; }
    public Color InstructionColor { get =>  instructionColor; }

    private void OnValidate()
    {
        cardType = CardType.Deck;
    }
}
