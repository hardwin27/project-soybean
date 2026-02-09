using UnityEngine;
using ReadOnlyEditor;
using System.Collections.Generic;

[System.Serializable]
public class StatIndicatorData
{
    [SerializeField] private StatData statData;
    [SerializeField] private Sprite statSprite;

    public StatData StatData { get => statData; }
    public Sprite StatSprite { get => statSprite; }
}

[RequireComponent(typeof(ToolCardController))]
public class ToolCardVisual : CardVisual
{
    [SerializeField, ReadOnly] private ToolCardController toolCardController;

    [SerializeField] protected Transform statIndicatorParent;
    [SerializeField] protected Sprite defaultSprite;
    [SerializeField] protected List<StatIndicatorData> statIndicators = new List<StatIndicatorData>();
    [SerializeField] protected Sprite multipleResourceFillled;

    protected override void Awake()
    {
        base.Awake();
        toolCardController = cardController as ToolCardController;
        toolCardController.OnToolDataUpdated += UpdateToolVisual;
    }

    protected void UpdateToolVisual()
    {
        Debug.Log($"{gameObject} Tool UpdateVisual");
        int filledResourceCount = 0;
        StatData targetedStat = null;

        foreach (var runtimeStat in toolCardController.RuntimeStats)
        {
            if (runtimeStat.CurrentValue > 0)
            {
                filledResourceCount++;
                targetedStat = runtimeStat.Stat;
            }
        }

        if (targetedStat != null) 
        { 
            if (filledResourceCount > 1)
            {
                cardRenderer.sprite = multipleResourceFillled;
                Debug.Log($"{gameObject} Tool ALL FILLED");
            }
            else
            {
                Debug.Log($"{gameObject} Tool One FILLED");
                StatIndicatorData statIndicatorData = statIndicators.Find(ind => ind.StatData == targetedStat);
                cardRenderer.sprite = (statIndicatorData == null) ? defaultSprite : statIndicatorData.StatSprite;
            }
        }
        else
        {
            Debug.Log($"{gameObject} Tool DEFAULT");
            cardRenderer.sprite = defaultSprite;
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
