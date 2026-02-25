using UnityEngine;
using ReadOnlyEditor;

[System.Serializable]
public class QuestTracker
{
    [SerializeField, ReadOnly] private QuestStatus questStatus;
    [SerializeField, ReadOnly] private int currentProgress;

    public QuestStatus QuestStatus { get => questStatus; }
    public int CurrentProgress { get => currentProgress; }

    public QuestTracker(QuestStatus _questStatus)
    {
        questStatus = _questStatus;
        currentProgress = 0;
    }

    public void AddProgress(int progress)
    {
        currentProgress += progress;
    }
}