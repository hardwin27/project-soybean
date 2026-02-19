using UnityEngine;

[CreateAssetMenu(menuName = "Card/Base Card Data")]
public class CardData : ScriptableObject 
{
    [SerializeField] protected string cardName;
    [SerializeField] protected GameObject cardPrefab;
    [SerializeField] protected int buyPrice;
    [SerializeField] protected int sellPrice;
    [SerializeField] protected float progressTime;

    [SerializeField] protected CardType cardType;
    [SerializeField] protected Sprite cardSprite;

    [SerializeField] protected bool useCustomHover;
    [SerializeField, TextArea] protected string customHoverMsg;

    public string CardName { get => cardName; }
    public GameObject CardPrefab { get => cardPrefab; }
    public int BuyPrice { get => buyPrice; }
    public int SellPrice { get => sellPrice; }
    public float ProgressTime { get => progressTime; }

    public CardType CardType { get => cardType; }
    public Sprite CardSprite { get => cardSprite; }
    public bool UseCustomHover { get => useCustomHover; }
    public string CustomHoverMsg { get => customHoverMsg; }
}
