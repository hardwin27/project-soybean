using UnityEngine;
using ReadOnlyEditor;

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
        /*Debug.Log($"UpdateToolVisual");*/
        foreach (var runtimeStat in toolCardController.RuntimeStats)
        {
            GameObject statIndicator = GenerateStatIndicator(runtimeStat.Stat);

            if (statIndicator != null) 
            {
                runtimeStat.OnValueUpdated += () =>
                {
                    ToggleIndicator(runtimeStat, statIndicator);
                };

                ToggleIndicator(runtimeStat, statIndicator);
            }
        }
    }

    protected void ToggleIndicator(RuntimeStat runtimeStat, GameObject statIndicator)
    {
        statIndicator.SetActive(runtimeStat.CurrentValue > 0);
    }

    protected GameObject GenerateStatIndicator(StatData statData)
    {
        GameObject statIndicator = null;

        if (statData.StatIndicatorSprite != null) 
        {
            statIndicator = new GameObject($"{statData.StatName} Indicator");
            statIndicator.transform.parent = statIndicatorParent;
            statIndicator.transform.localPosition = statData.StatIndicatorPos;
            statIndicator.transform.localScale = statData.StatIndicatorSize;
            
            SpriteRenderer statRenderer = statIndicator.AddComponent<SpriteRenderer>();
            statRenderer.sprite = statData.StatIndicatorSprite;
        }

        return statIndicator;
    }
}
