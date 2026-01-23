using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeCardData
{
    [SerializeField] private CardData cardData;
    [SerializeField] private Sprite recipeCardSprite;

    public CardData CardData { get { return cardData; } }
    public Sprite RecipeCardSprite { get { return recipeCardSprite; } }
}

[CreateAssetMenu(menuName = "Recipe")]
public class RecipeData : ScriptableObject
{
    [SerializeField] private List<RecipeCardData> cardCombos = new List<RecipeCardData>();
    [SerializeField] private CardData topCardReq;
    [SerializeField] private List<RecipeToolReq> requiredTools = new List<RecipeToolReq>();
    [SerializeField] private float processDuration;
    [SerializeField] private CardData generatedCard;
    [SerializeField] private int generatedCardAmount = 1;
    [SerializeField] private List<CardData> destroyedCards;
    [SerializeField] private List<RecipeToolReq> toolChanges = new List<RecipeToolReq>();
    [SerializeField] private Sprite recipeTargetSprite;

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
    public int GeneratedCardAmount { get => generatedCardAmount; }
    public List<CardData> DestroyedCards { get => destroyedCards; }
    public List<RecipeToolReq> ToolChanges { get => toolChanges; }
    public Sprite RecipeTargetSprite { get => recipeTargetSprite; }
}
