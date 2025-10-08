using UnityEngine;

[CreateAssetMenu(fileName = "UiQuestData", menuName = "Quest/UI Quest Data")]
public class UiQuestData : ScriptableObject
{
    [SerializeField] protected string targetUiId;

    public string TargetUiId { get => targetUiId; }
};
