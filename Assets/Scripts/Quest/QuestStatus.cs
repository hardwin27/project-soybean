using Sirenix.OdinInspector;
using System;
using UnityEngine;

[System.Serializable]
public class QuestStatus
{
    [SerializeField] private QuestData questData;
    [SerializeField, ReadOnly] private bool isCompleted;

    public Action OnQuestUpdated;

    public QuestData QuestData { get => questData; }
    public bool IsCompleted { get => isCompleted; }

    public QuestStatus(QuestData questData, bool isCompleted)
    {
        this.questData = questData;
        this.isCompleted = isCompleted;
    }

    public void ToggleQuest(bool value)
    {
        isCompleted = value;
        OnQuestUpdated?.Invoke();
    }
}
