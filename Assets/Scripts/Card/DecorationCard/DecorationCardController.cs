using UnityEngine;
using ReadOnlyEditor;
using UnityEngine.EventSystems;

public class DecorationCardController : CardController, IPointerClickHandler
{
    [SerializeField, ReadOnly] private DecorationCardData decorationCardData;

    public override void AssignCardData(CardData data)
    {
        base.AssignCardData(data);
        decorationCardData = data as DecorationCardData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            gameObject.SetActive(false);
        }
    }

    public void StartDraggedByUi()
    {
        /*Debug.Log("StartDraggedByUi");*/
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
