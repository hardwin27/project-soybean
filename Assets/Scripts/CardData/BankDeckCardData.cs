using UnityEngine;

[CreateAssetMenu(menuName = "Card/Bank Deck Card Data")]
public class BankDeckCardData : DeckCardData
{
    [SerializeField] protected int maxMoney;

    public int MaxMoney { get => maxMoney; }
}
