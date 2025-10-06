using UnityEngine;
using System.Linq;
using SingletonSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class CardComboManager : Singleton<CardComboManager>
{
    [SerializeField] private List<RecipeData> recipes = new List<RecipeData>();

    [SerializeField, ReadOnly] private List<CardController> cards = new List<CardController>();

    private void Awake()
    {
        cards = FindObjectsByType<CardController>(FindObjectsSortMode.None).ToList();
        foreach (var card in cards)
        {
            card.OnCardStacked += CheckStackedCard;
        }
    }

    private void CheckStackedCard(CardController stackedCard)
    {
        List<CardController> cardStack = new List<CardController>();

        CardController card = stackedCard;
        while (card != null) 
        {
            cardStack.Add(card);
            card = card.StackedOnCard;
        }

        RecipeData selectedRecipe = null;
        foreach (var recipe in  recipes) 
        {
            if (recipe.TopCardReq == stackedCard.CardData)
            {
                if (CheckReqToolPass(recipe, cardStack))
                {
                    if (CheckCombo(recipe, cardStack))
                    {
                        selectedRecipe = recipe;
                        break;
                    }
                }
            }
        }

        if (selectedRecipe != null) 
        {
            Debug.Log($"COMBO {selectedRecipe.name}");
        }
        else
        {
            Debug.Log($"No Combo Detected");
        }
    }

    private bool CheckCombo(RecipeData recipe, List<CardController> cardStack)
    {
        if (recipe.CardCombos.Count != cardStack.Count)
        {
            Debug.Log($"Recipe {recipe.name} have different stack");
            return false;
        }

        List<CardData> cardDataOnStack = new List<CardData>();
        foreach (var card in cardStack)
        {
            cardDataOnStack.Add(card.CardData);
        }

        return !cardDataOnStack.Except(recipe.CardCombos).Any() && !recipe.CardCombos.Except(cardDataOnStack).Any();
    }

    private bool CheckReqToolPass(RecipeData recipe, List<CardController> cardStack)
    {
        if (recipe.RequiredTools.Count <= 0)
        {
            Debug.Log($"{recipe.name} True, No Req Tool");
            return true;
        }

        foreach (var reqTool in recipe.RequiredTools)
        {
            CardController cardController = cardStack.Find(card => card.CardData == reqTool.ToolCard);
            ToolCardController toolCard = cardController as ToolCardController;

            if (toolCard == null)
            {
                Debug.Log($"{recipe.name} False, {reqTool.ToolCard.CardName} not found");
                return false;
            }

            // Compare all req stat
            foreach (var reqToolStat in reqTool.ToolStats)
            {
                RuntimeStat stat = toolCard.RuntimeStats.Find(s => s.Stat == reqToolStat.StatData);
                if (stat == null)
                {
                    Debug.Log($"{recipe.name} False, {reqTool.ToolCard.CardName} stat {reqToolStat.StatData.StatName} not found");
                    return false;
                }

                if (stat.CurrentValue != reqToolStat.StatValue)
                {
                    Debug.Log($"{recipe.name} False, {reqTool.ToolCard.CardName} stat {reqToolStat.StatData.StatName} value diff");
                    return false;
                }
            }
        }

        Debug.Log($"{recipe.name} True, All Tool Clear");
        return true;
    }
}
