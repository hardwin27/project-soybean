using UnityEngine;

[CreateAssetMenu(fileName = "BankQuestData", menuName = "Quest/Bank Quest Data")]
public class BankQuestData : QuestData
{
    [SerializeField] protected int targetTotalMoney;

    public int TargetTotalMoney { get => targetTotalMoney; }
}
