using UnityEngine;

[CreateAssetMenu(menuName = "Card/Base Card Data")]
public class CardData : ScriptableObject 
{
    [SerializeField] protected string cardName;
    [SerializeField] protected int buyPrice;
    [SerializeField] protected int sellPrice;
    [SerializeField] protected float progressTime;

    [SerializeField] protected CardType cardType;
    [SerializeField] protected Sprite cardSprite;

    public string CardName { get => cardName; }
    public int BuyPrice { get => buyPrice; }
    public int SellPrice { get => sellPrice; }
    public float ProgressTime { get => progressTime; }

    public CardType CardType { get => cardType; }
    public Sprite CardSprite { get => cardSprite; }
}
