using UnityEngine;

[CreateAssetMenu(fileName = "GeneratedCardQuestData", menuName = "Quest/Generated Card Quest Data")]
public class GeneratedCardQuestData : QuestData
{
    [SerializeField] private CardData targetGeneratedCard;

    public CardData TargetGeneratedCard { get => targetGeneratedCard; }
}
