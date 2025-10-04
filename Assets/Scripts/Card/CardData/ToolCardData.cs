using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Card/Tool Card Data")]
public class ToolCardData : CardData
{
    [SerializeField] protected List<StatData> statDatas = new List<StatData>();

    public List<StatData> StatDatas { get => statDatas; }

    private void OnValidate()
    {
        cardType = CardType.Tool;
    }
}
