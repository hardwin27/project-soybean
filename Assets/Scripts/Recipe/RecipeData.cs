using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeCardData
{
    [SerializeField] private CardData cardData;
    [SerializeField] private Sprite recipeCardSprite;
    [SerializeField] private string recipeCardName = null;

    public CardData CardData { get { return cardData; } }
    public Sprite RecipeCardSprite { get { return recipeCardSprite; } }
    public string RecipeCardName { get {  return recipeCardName; } }
}

[System.Serializable]
public class GeneratedCardData
{
    [SerializeField] private CardData cardData;
    [SerializeField] private int generatedQty;
    [SerializeField] private Sprite generatedCardSprite;
    [SerializeField] private string generatedCardName = null;

    public CardData CardData { get => cardData; }
    public int GeneratedQty { get => generatedQty; }
    public Sprite GeneratedCardSprite { get => generatedCardSprite; }
    public string GeneratedCardName { get => generatedCardName; }
}

[CreateAssetMenu(menuName = "Recipe")]
public class RecipeData : ScriptableObject
{
    [SerializeField] private List<RecipeCardData> cardCombos = new List<RecipeCardData>();
    [SerializeField] private CardData topCardReq;
    [SerializeField] private List<RecipeToolReq> requiredTools = new List<RecipeToolReq>();
    [SerializeField] private float processDuration;
    [SerializeField] private CardData generatedCard;
    [SerializeField] private List<GeneratedCardData> generatedCards = new List<GeneratedCardData>();
    [SerializeField] private int generatedCardAmount = 1;
    [SerializeField] private List<CardData> destroyedCards;
    [SerializeField] private List<RecipeToolReq> toolChanges = new List<RecipeToolReq>();
    [SerializeField] private Sprite recipeTargetSprite;
    [SerializeField] private string recipeSpecialTargetName;

    public List<RecipeCardData> CardCombos { get => cardCombos; }
    public CardData TopCardReq { get => topCardReq; }
    public List<RecipeToolReq> RequiredTools { get => requiredTools; }
    public float ProcessDuration
    {
        get
        {
            float duration = 0f;

            foreach (var card in cardCombos)
            {
                duration += card.CardData.ProgressTime;
            }

            return duration;
        }

    }
    public CardData GeneratedCard { get => generatedCard; }
    public List<GeneratedCardData> GeneratedCards { get => generatedCards; }
    public int GeneratedCardAmount { get => generatedCardAmount; }
    public List<CardData> DestroyedCards { get => destroyedCards; }
    public List<RecipeToolReq> ToolChanges { get => toolChanges; }
    public Sprite RecipeTargetSprite { get => recipeTargetSprite; }
    public string RecipeSpecialTargetName { get => recipeSpecialTargetName; }

    public int RecipeTargetCount => (GeneratedCards.Count > 0) ? GeneratedCards.Count : 1;
}
