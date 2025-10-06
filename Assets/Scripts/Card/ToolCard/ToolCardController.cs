using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;

public class ToolCardController : CardController
{
    [SerializeField, ReadOnly] private ToolCardData toolCardData;
    [SerializeField, ReadOnly] private List<RuntimeStat> runtimeStats = new List<RuntimeStat>();

    public Action OnToolDataUpdated;

    public ToolCardData ToolCardData { get => toolCardData; }
    public List<RuntimeStat> RuntimeStats { get => runtimeStats; }


    public override void AssignCardData(CardData data)
    {
        base.AssignCardData(data);
        toolCardData = data as ToolCardData;
        runtimeStats.Clear();
        foreach (var stat in toolCardData.StatDatas)
        {
            var runtimeStat = new RuntimeStat(stat);
            runtimeStats.Add(runtimeStat);
        }
        OnToolDataUpdated?.Invoke();

        print($"ASSIGN TOOL {data.CardName}");
    }

    protected void ResetStat()
    {
        foreach (var runtimeStat in runtimeStats) 
        {
            runtimeStat.SetValue(0);
        }
    }
}
