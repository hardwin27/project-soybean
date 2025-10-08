using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

// Hardcode quest for now
public class QuestController : MonoBehaviour
{
    [SerializeField] private List<QuestStatus> quests = new List<QuestStatus>();

    [Title("Component for Quest")]
    [SerializeField, ReadOnly] protected CardComboManager cardComboManager;
    [SerializeField] private DeckSellCardController sellDeck;
    [SerializeField] private DeckBankCardController bankDeck;
    [SerializeField] private GameplayUiController gameplayUi;

    public Action<QuestStatus> OnQuestAdded;

    private void Awake()
    {
        cardComboManager = CardComboManager.Instance;

        cardComboManager.OnRecipeProcessed += HandleRecipeProcessed;
        sellDeck.OnCardSold += HandleCardSold;
        bankDeck.OnDeckBankDatUpdated += HandleBankUpdated;
        gameplayUi.OnUiTriggered += HandleUiUsed;
    }

    private void Start()
    {
        foreach(var quest in quests)
        {
            OnQuestAdded?.Invoke(quest);
        }
    }

    private void HandleRecipeProcessed(RecipeData recipe)
    {
        foreach (var quest in quests) 
        {
            if (!quest.IsCompleted)
            {
                if (quest.QuestData is RecipeQuestData)
                {
                    RecipeQuestData recipeQuestData = (RecipeQuestData)quest.QuestData;
                    if (recipeQuestData.TargetRecipe == recipe)
                    {
                        quest.ToggleQuest(true);
                    }
                }
            }
        }
    }

    private void HandleCardSold(CardController cardController)
    {
        foreach (var quest in quests)
        {
            if (!quest.IsCompleted)
            {
                if (quest.QuestData is SellQuestData)
                {
                    SellQuestData sellQuestData = (SellQuestData)quest.QuestData;
                    if (sellQuestData.TargetSoldCard == cardController.CardData)
                    {
                        quest.ToggleQuest(true);
                    }
                }
            }
        }
    }

    private void HandleBankUpdated()
    {
        foreach (var quest in quests)
        {
            if (!quest.IsCompleted)
            {
                if (quest.QuestData is BankQuestData)
                {
                    BankQuestData bankQuestData = (BankQuestData)quest.QuestData;
                    if (bankDeck.CurrentMoney >= bankQuestData.TargetTotalMoney)
                    {
                        quest.ToggleQuest(true);
                    }
                }
            }
        }
    }

    private void HandleUiUsed(string uiId)
    {
        foreach (var quest in quests)
        {
            if (!quest.IsCompleted)
            {
                if (quest.QuestData is UiQuestData)
                {
                    UiQuestData uiQuestData = (UiQuestData)quest.QuestData;
                    if (uiId == uiQuestData.TargetUiId)
                    {
                        quest.ToggleQuest(true);
                    }
                }
            }
        }
    }
}