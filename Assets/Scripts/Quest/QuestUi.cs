using UnityEngine;

public class QuestUi : MonoBehaviour
{
    [SerializeField] private QuestController questController;
    [SerializeField] private GameObject questChapterPrefab;
    [SerializeField] private GameObject questEntryPrefab;
    [SerializeField] private Transform questEntryParent;

    private void Awake()
    {
        /*questController.OnQuestAdded += AddQuestEntry;*/
        questController.OnQuestChapterAdded += AddQuesChapter;
    }

    public void AddQuestEntry(QuestStatus questStatus)
    {
        GameObject questEntryObj = Instantiate(questEntryPrefab, questEntryParent);
        if (questEntryObj.TryGetComponent(out QuestEntryUi questEntryUi))
        {
            questEntryUi.AssignQuest(questStatus);
        }
    }

    public void AddQuesChapter(QuestChapter chapter)
    {
        GameObject chapterObject = Instantiate(questChapterPrefab, questEntryParent);
        if (chapterObject.TryGetComponent(out QuestChapterUi questChapterUi))
        {
            questChapterUi.AssignChapterTitle(chapter.ChapterName);
        }
        foreach (var quest in chapter.Quests)
        {
            AddQuestEntry(quest);
        }
    }
}
