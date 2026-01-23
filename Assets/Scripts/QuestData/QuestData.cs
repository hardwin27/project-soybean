using UnityEngine;

/*[CreateAssetMenu(fileName = "QuestData", menuName = "Quest/Quest Data")]*/
public class QuestData : ScriptableObject
{
    [SerializeField] protected string questTitle;
    [SerializeField] protected bool hasReward;

    public string QuestTitle { get => questTitle;  }
    public bool HasReward { get => hasReward; }
}
