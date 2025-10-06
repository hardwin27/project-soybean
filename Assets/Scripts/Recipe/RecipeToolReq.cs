using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeToolReq
{
    [SerializeField] private ToolCardData toolCard;
    [SerializeField] private List<RecipeToolReqStat> toolStats = new List<RecipeToolReqStat>();

    public ToolCardData ToolCard { get => toolCard; }
    public List<RecipeToolReqStat> ToolStats { get => toolStats; }
}

[System.Serializable]
public class RecipeToolReqStat
{
    [SerializeField] private StatData statData;
    [SerializeField] private int statValue;

    public StatData StatData { get => statData; }
    public int StatValue { get => statValue; }
}
