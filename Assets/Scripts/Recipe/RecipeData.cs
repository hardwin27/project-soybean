using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Recipe")]
public class RecipeData : ScriptableObject
{
    [SerializeField] private List<CardData> cardCombos = new List<CardData>();
    [SerializeField] private CardData topCardReq;
    [SerializeField] private List<RecipeToolReq> requiredTools = new List<RecipeToolReq>();
    [SerializeField] private CardData generatedCard;
    [SerializeField] private List<CardData> destroyedCards;
    [SerializeField] private List<RecipeToolReq> toolChanges = new List<RecipeToolReq>();

    public List<CardData> CardCombos { get => cardCombos; }
    public CardData TopCardReq { get => topCardReq; }
    public List<RecipeToolReq> RequiredTools { get => requiredTools; }
    public CardData GeneratedCard { get => generatedCard; }
    public List<CardData> DestroyedCards { get => destroyedCards; }
    public List<RecipeToolReq> ToolChanges { get => toolChanges; }
}
