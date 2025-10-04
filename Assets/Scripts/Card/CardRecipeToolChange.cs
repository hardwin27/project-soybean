using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardRecipeToolChange
{
    [SerializeField] private ToolCardData toolCard;
    [SerializeField] private List<ToolStatChange> toolStatChanges = new List<ToolStatChange>();

    public ToolCardData ToolCard { get => toolCard; }
    public List<ToolStatChange> ToolStatChanges { get => toolStatChanges; }
}

[System.Serializable]
public class ToolStatChange
{
    [SerializeField] private StatData statData;
    [SerializeField] private int statValueChange;

    public StatData StatData { get => statData; }
    public int StatValueChange { get => statValueChange; }
}
