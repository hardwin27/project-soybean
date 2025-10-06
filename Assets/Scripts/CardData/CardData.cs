using UnityEngine;

[CreateAssetMenu(menuName = "Card/Base Card Data")]
public class CardData : ScriptableObject 
{
    [SerializeField] protected string cardName;
    [SerializeField] protected CardType cardType;
    [SerializeField] protected Sprite cardSprite;

    public string CardName { get => cardName; }
    public CardType CardType { get => cardType; }
    public Sprite CardSprite { get => cardSprite; }
}
