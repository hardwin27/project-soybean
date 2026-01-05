using UnityEngine;
using System;

[System.Serializable]
public class RuntimeStat
{
    [SerializeField] private StatData stat;
    [SerializeField] private int currentValue;

    public StatData Stat { get => stat; }
    public int CurrentValue { get => currentValue;  }

    public Action OnValueUpdated;

    public RuntimeStat(StatData _stat)
    {
        stat = _stat;
        SetValue(0);
    }

    public void SetValue(int value)
    {
        currentValue = value;
        OnValueUpdated?.Invoke();
    }
}
