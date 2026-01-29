using UnityEngine;
using ReadOnlyEditor;

public class DecorationCardController : CardController
{
    [SerializeField, ReadOnly] private DecorationCardData decorationCardData;

    protected void OnMouseUp()
    {
        
    }

    public override void AssignCardData(CardData data)
    {
        base.AssignCardData(data);
        decorationCardData = data as DecorationCardData;
    }

    public void StartDraggedByUi()
    {
        Debug.Log("StartDraggedByUi");
        cardSorting.sortingLayerName = "DraggedUi";
        cardCollider.enabled = false;
    }

    public void StopDraggedByUi()
    {
        cardSorting.sortingLayerName = cardDefaultSortName;
        cardCollider.enabled = true;
        StartCoroutine(DelayedOverlapCheck());
    }
}
