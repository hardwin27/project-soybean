using UnityEngine;

[CreateAssetMenu(menuName = "Card/Decoration Card Data")]
public class DecorationCardData : CardData
{
    [SerializeField] private Sprite uiSprite;

    public Sprite UiSprite { get => uiSprite; }

    private void OnValidate()
    {
        cardType = CardType.Decoration;
    }
}
