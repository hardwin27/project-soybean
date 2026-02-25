using UnityEngine;
using ReadOnlyEditor;

// [System.Serializable]
// public class SellQuestTracker
// {
//     [SerializeField, ReadOnly] private SellQuestData sellQuestData;
//     [SerializeField, ReadOnly] private int currentProgress;

//     public SellQuestData SellQuestData { get => sellQuestData; }
//     public int CurrentProgress { get => currentProgress; }

//     public SellQuestTracker(SellQuestData _sellQuestData)
//     {
//         sellQuestData = _sellQuestData;
//         currentProgress = 0;
//     }

//     public void AddProgress(int progress)
//     {
//         currentProgress += progress;
//     }
// }

[CreateAssetMenu(fileName = "SellQuestData", menuName = "Quest/Sell Quest Data")]
public class SellQuestData : QuestData
{
    [SerializeField] protected CardData targetSoldCard;
    [SerializeField] protected int targetSoldAmount = 1;

    public CardData TargetSoldCard { get => targetSoldCard; }
    public int TargetSoldAmount { get => targetSoldAmount; }
}
