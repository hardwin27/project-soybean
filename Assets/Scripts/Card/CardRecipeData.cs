using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Card Recipe")]
public class CardRecipeData : ScriptableObject
{
    [SerializeField] private List<CardData> cardCombos = new List<CardData>();
    [SerializeField] private CardData topCardReq;
    [SerializeField] private CardData generatedCard;
    [SerializeField] private List<CardData> destroyedCards;
    [SerializeField] private List<CardRecipeToolChange> cardRecipeToolChanges = new List<CardRecipeToolChange>();

    public List<CardData> CardCombos { get => cardCombos; }
    public CardData TopCardReq { get => topCardReq; }
    public CardData GeneratedCard { get => generatedCard; }
    public List<CardData> DestroyedCards { get => destroyedCards; }
    public List<CardRecipeToolChange> CardRecipeToolChanges { get => cardRecipeToolChanges; }
}
