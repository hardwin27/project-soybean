using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class RuntimeStat
{
    [SerializeField] private StatData stat;
    [SerializeField, ReadOnly] private int currentValue;

    public StatData Stat { get => stat; }
    public int CurrentValue { get => currentValue; set => currentValue = value; }
}
