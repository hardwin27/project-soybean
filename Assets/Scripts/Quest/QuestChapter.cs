using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestChapter
{
    [SerializeField] private string chapterName;
    [SerializeField] private List<QuestStatus> quests = new List<QuestStatus>();

    public string ChapterName { get { return chapterName; } }
    public List<QuestStatus> Quests { get {  return quests; } }
}
