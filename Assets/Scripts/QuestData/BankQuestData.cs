using UnityEngine;

[CreateAssetMenu(fileName = "BankQuestData", menuName = "Scriptable Objects/BankQuestData")]
public class BankQuestData : ScriptableObject
{
    [SerializeField] protected int targetTotalMoney;

    public int TargetTotalMoney { get => targetTotalMoney; }
}
