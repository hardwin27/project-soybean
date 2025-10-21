using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

// Hardcode quest for now
public class QuestController : MonoBehaviour
{
    /*[SerializeField] private List<QuestStatus> quests = new List<QuestStatus>();*/
    [SerializeField] private List<QuestChapter> questChapters = new List<QuestChapter>();

    [Title("Component for Quest")]
    [SerializeField, ReadOnly] protected CardComboManager cardComboManager;
    [SerializeField, ReadOnly] protected CardGeneratorManager cardGeneratorManager;
    [SerializeField] private DeckSellCardController sellDeck;
    [SerializeField] private DeckBankCardController bankDeck;
    [SerializeField] private GameplayUiController gameplayUi;

    public Action<QuestStatus> OnQuestAdded;
    public Action<QuestChapter> OnQuestChapterAdded;
    public Action OnLastQuestCompleted;

    private void Awake()
    {
        cardComboManager = CardComboManager.Instance;
        cardGeneratorManager = CardGeneratorManager.Instance;
        cardComboManager.OnRecipeProcessed += HandleRecipeProcessed;
        cardGeneratorManager.OnCardGenerated += HandleCardGenerated;
        sellDeck.OnCardSold += HandleCardSold;
        bankDeck.OnDeckBankDatUpdated += HandleBankUpdated;
        gameplayUi.OnUiTriggered += HandleUiUsed;
    }

    private void Start()
    {
        /*foreach(var quest in quests)
        {
            OnQuestAdded?.Invoke(quest);
        }*/

        foreach (var chapter in questChapters)
        {
            OnQuestChapterAdded?.Invoke(chapter);
        }

        questChapters[questChapters.Count - 1].Quests[questChapters[questChapters.Count - 1].Quests.Count - 1].OnQuestUpdated += () =>
        {
            if (questChapters[questChapters.Count - 1].Quests[questChapters[questChapters.Count - 1].Quests.Count - 1].IsCompleted)
            {
                OnLastQuestCompleted?.Invoke();
            }
        };
    }

    private void HandleRecipeProcessed(RecipeData recipe)
    {
        foreach (var chapter in questChapters)
        {
            foreach (var quest in chapter.Quests)
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
    }

    private void HandleCardGenerated(CardController cardController) 
    {
        foreach (var chapter in questChapters)
        {
            foreach (var quest in chapter.Quests)
            {
                if (!quest.IsCompleted)
                {
                    if (quest.QuestData is GeneratedCardQuestData)
                    {
                        GeneratedCardQuestData generatedCardQuestData = (GeneratedCardQuestData)quest.QuestData;
                        if (generatedCardQuestData.TargetGeneratedCard == cardController.CardData)
                        {
                            quest.ToggleQuest(true);
                        }
                    }
                }
            }
        }
    }

    private void HandleCardSold(CardController cardController)
    {
        foreach (var chapter in questChapters)
        {
            foreach (var quest in chapter.Quests)
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
    }

    private void HandleBankUpdated()
    {
        foreach (var chapter in questChapters)
        {
            foreach (var quest in chapter.Quests)
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
    }

    private void HandleUiUsed(string uiId)
    {
        foreach (var chapter in questChapters)
        {
            foreach (var quest in chapter.Quests)
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
}