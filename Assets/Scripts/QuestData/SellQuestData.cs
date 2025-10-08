using UnityEngine;

[CreateAssetMenu(fileName = "SellQuestData", menuName = "Quest/Sell Quest Data")]
public class SellQuestData : QuestData
{
    [SerializeField] protected CardData targetSoldCard;

    public CardData TargetSoldCard { get => targetSoldCard; }
}
