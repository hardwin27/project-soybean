using UnityEngine;

[System.Serializable]
public class CardOnDeckData
{
    [SerializeField] private CardData cardData;
    [SerializeField] private Sprite cardOnDeckSprite;

    public CardData CardData { get => cardData; }
    public Sprite CardOnDeckSprite { get => cardOnDeckSprite; }
}
