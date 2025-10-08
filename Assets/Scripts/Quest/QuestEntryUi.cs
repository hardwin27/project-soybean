using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestEntryUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questTitleText;
    [SerializeField] private Image questIndicatorImage;
    [SerializeField] private Sprite questIncompleteSprite;
    [SerializeField] private Sprite questCompleteSprite;

    protected QuestStatus questStatus;

    public void AssignQuest(QuestStatus quest)
    {
        questStatus = quest;
        questTitleText.text = questStatus.QuestData.QuestTitle;
        questStatus.OnQuestUpdated += UpdateUi;
        UpdateUi();
    }

    public void UpdateUi()
    {
        questIndicatorImage.sprite = (questStatus.IsCompleted) ? questCompleteSprite : questIncompleteSprite;
    }
}
