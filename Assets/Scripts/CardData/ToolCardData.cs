using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Card/Tool Card Data")]
public class ToolCardData : CardData
{
    [SerializeField] protected List<StatData> statDatas = new List<StatData>();
    [SerializeField] protected Sprite withResourceSprite;
    public List<StatData> StatDatas { get => statDatas; }
    public Sprite WithResourceSprite { get => withResourceSprite; }

    private void OnValidate()
    {
        cardType = CardType.Tool;
    }
}
