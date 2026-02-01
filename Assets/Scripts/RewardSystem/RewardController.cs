using UnityEngine;

public class RewardController : MonoBehaviour
{
    [SerializeField] private QuestController questController;
    private DecorationManager decorationManager;

    private void Awake()
    {
        decorationManager = DecorationManager.Instance;

        questController.OnQuestChapterAdded += HandleQuestChapterAdded;
    }

    private void HandleQuestChapterAdded(QuestChapter questChapter)
    {
        foreach (var questStatus in questChapter.Quests)
        {
            if (questStatus.QuestData.HasReward)
            {
                Debug.Log($"Handle Quest With {questStatus.QuestData.QuestDecorationReward.DecorationReward.CardName} reward");
                questStatus.OnQuestUpdated += () =>
                {
                    if (questStatus.IsCompleted)
                    {
                        Debug.Log($"Quest With Reward Completed");
                        QuestDecorationReward questDecorationReward = questStatus.QuestData.QuestDecorationReward;
                        decorationManager.GainDecoration(questDecorationReward.DecorationReward,
                            questDecorationReward.RewardAmount);
                    }
                };
            }
        }
    }
}
