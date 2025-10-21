using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestEntryUi : MonoBehaviour
{
    [SerializeField] private RectTransform questRect;
    [SerializeField] private TextMeshProUGUI questTitleText;
    [SerializeField] private Image questIndicatorImage;
    [SerializeField] private Sprite questIncompleteSprite;
    [SerializeField] private Sprite questCompleteSprite;

    [SerializeField, ReadOnly] private Bounds textBound;

    protected QuestStatus questStatus;

    public void AssignQuest(QuestStatus quest)
    {
        questStatus = quest;
        questTitleText.text = questStatus.QuestData.QuestTitle;
        questStatus.OnQuestUpdated += UpdateUi;

        questTitleText.ForceMeshUpdate();

        float height = questTitleText.preferredHeight;

        questRect.sizeDelta = new Vector2(questRect.sizeDelta.x, height);

        UpdateUi();
    }

    public void UpdateUi()
    {
        questIndicatorImage.sprite = (questStatus.IsCompleted) ? questCompleteSprite : questIncompleteSprite;
    }
}
