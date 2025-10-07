using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Recipe")]
public class RecipeData : ScriptableObject
{
    [SerializeField] private List<CardData> cardCombos = new List<CardData>();
    [SerializeField] private CardData topCardReq;
    [SerializeField] private List<RecipeToolReq> requiredTools = new List<RecipeToolReq>();
    [SerializeField] private float processDuration;
    [SerializeField] private CardData generatedCard;
    [SerializeField] private int generatedCardAmount = 1;
    [SerializeField] private List<CardData> destroyedCards;
    [SerializeField] private List<RecipeToolReq> toolChanges = new List<RecipeToolReq>();

    public List<CardData> CardCombos { get => cardCombos; }
    public CardData TopCardReq { get => topCardReq; }
    public List<RecipeToolReq> RequiredTools { get => requiredTools; }
    public float ProcessDuration { get => processDuration; }
    public CardData GeneratedCard { get => generatedCard; }
    public int GeneratedCardAmount { get => generatedCardAmount; }
    public List<CardData> DestroyedCards { get => destroyedCards; }
    public List<RecipeToolReq> ToolChanges { get => toolChanges; }
}
