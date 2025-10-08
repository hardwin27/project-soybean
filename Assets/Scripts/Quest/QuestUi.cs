using UnityEngine;

public class QuestUi : MonoBehaviour
{
    [SerializeField] private QuestController questController;
    [SerializeField] private GameObject questEntryPrefab;
    [SerializeField] private Transform questEntryParent;

    private void Awake()
    {
        questController.OnQuestAdded += AddQuestEntry;
    }

    public void AddQuestEntry(QuestStatus questStatus)
    {
        GameObject questEntryObj = Instantiate(questEntryPrefab, questEntryParent);
        if (questEntryObj.TryGetComponent(out QuestEntryUi questEntryUi))
        {
            questEntryUi.AssignQuest(questStatus);
        }
    }
}
