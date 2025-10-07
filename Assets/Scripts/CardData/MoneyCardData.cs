using UnityEngine;

[CreateAssetMenu(menuName = "Card/Money Card Data")]
public class MoneyCardData : CardData
{
    [SerializeField] int moneyValue;

    public int MoneyValue { get => moneyValue; }

    private void OnValidate()
    {
        cardType = CardType.Money;
    }
}
