using UnityEngine;

[CreateAssetMenu(fileName = "UiQuestData", menuName = "Quest/UI Quest Data")]
public class UiQuestData : QuestData
{
    [SerializeField] protected string targetUiId;

    public string TargetUiId { get => targetUiId; }
};
