using UnityEngine;
using Sirenix.OdinInspector;
using System;

[System.Serializable]
public class RuntimeStat
{
    [SerializeField] private StatData stat;
    [SerializeField, ReadOnly] private int currentValue;

    public StatData Stat { get => stat; }
    public int CurrentValue { get => currentValue;  }

    public Action OnValueUpdated;

    public void SetValue(int value)
    {
        currentValue = value;
        OnValueUpdated?.Invoke();
    }
}
