using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(ToolCardController))]
public class ToolCardVisual : CardVisual
{
    [SerializeField, ReadOnly] private ToolCardController toolCardController;

    [SerializeField] protected Transform statIndicatorParent;

    protected override void Awake()
    {
        base.Awake();
        toolCardController = cardController as ToolCardController;
        toolCardController.OnToolDataUpdated += UpdateToolVisual;
    }

    protected void UpdateToolVisual()
    {
        foreach (var runtimeStat in toolCardController.RuntimeStats)
        {
            runtimeStat.OnValueUpdated += UpdateStatIndicator;
        }
    }

    protected void UpdateStatIndicator()
    {

    }
}
