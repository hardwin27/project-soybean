using UnityEngine;

[System.Serializable]
public class QuestDecorationReward
{
    [SerializeField] private DecorationCardData decorationReward;
    [SerializeField] private int rewardAmount;

    public DecorationCardData DecorationReward { get => decorationReward; }
    public int RewardAmount { get => rewardAmount; }
}

/*[CreateAssetMenu(fileName = "QuestData", menuName = "Quest/Quest Data")]*/
public class QuestData : ScriptableObject
{
    [SerializeField] protected string questTitle;
    [SerializeField] protected QuestDecorationReward questDecorationReward = null;

    public string QuestTitle { get => questTitle;  }
    public bool HasReward 
    { 
        get
        {
            if (questDecorationReward != null)
            {
                return (questDecorationReward.DecorationReward != null);
            }
            return false;
        }
    }
    public QuestDecorationReward QuestDecorationReward { get => questDecorationReward; }
}
