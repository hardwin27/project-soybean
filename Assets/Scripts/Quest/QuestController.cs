using System.Collections.Generic;
using UnityEngine;
using System;
using ReadOnlyEditor;

// Hardcode quest for now
public class QuestController : MonoBehaviour
{
    /*[SerializeField] private List<QuestStatus> quests = new List<QuestStatus>();*/
    [SerializeField] private List<QuestChapter> questChapters = new List<QuestChapter>();

    [SerializeField, ReadOnly] protected CardComboManager cardComboManager;
    [SerializeField, ReadOnly] protected CardGeneratorManager cardGeneratorManager;
    [SerializeField] private BuildingDeckSellCardController sellDeck;
    [SerializeField] private BuildingDeckBankCardController bankDeck;
    [SerializeField] private GameplayUiController gameplayUi;

    [SerializeField, ReadOnly] private List<SellQuestTracker> sellQuestTrackers = new List<SellQuestTracker>();

    /*public Action<QuestStatus> OnQuestAdded;*/
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
        bankDeck.OnDeckCardGenerated += HandleBankWithdraw;
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
            foreach(var questData in chapter.Quests)
            {
                if (questData.QuestData is SellQuestData)
                {
                    SellQuestData sellQuestData = questData.QuestData as SellQuestData;
                    sellQuestTrackers.Add(new SellQuestTracker(sellQuestData));
                }
            }
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
                        SellQuestTracker sellQuestTracker = sellQuestTrackers.Find(tracker => tracker.SellQuestData.TargetSoldCard == cardController.CardData);
                        if (sellQuestTracker != null)
                        {
                            sellQuestTracker.AddProgress(1);
                            if (sellQuestTracker.CurrentProgress >= sellQuestData.TargetSoldAmount)
                            {
                                quest.ToggleQuest(true);
                            }
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

    private void HandleBankWithdraw(CardData cardData)
    {
        foreach (var chapter in questChapters)
        {
            foreach (var quest in chapter.Quests)
            {
                if (!quest.IsCompleted)
                {
                    if (quest.QuestData is BankWithdrawQuestData)
                    {
                        BankWithdrawQuestData bankWithdrawQuestData = (BankWithdrawQuestData)quest.QuestData;
                        quest.ToggleQuest(true);
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