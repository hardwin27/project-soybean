using UnityEngine;

public class BuildingCardVisual : CardVisual
{
    protected override void Awake()
    {
        base.Awake();
        isReqSpriteAssg = false;
    }
}
