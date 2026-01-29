using UnityEngine;
using UnityEngine.UI;
using ReadOnlyEditor;
using UnityEngine.EventSystems;
using TMPro;

public class DecorationOptionUi : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private DecorationCardData decorationCardData;
    [SerializeField, ReadOnly] private DecorationCardController currentDecorationCard;

    [SerializeField] private Image decorationIcon;
    [SerializeField] private TextMeshProUGUI decorationNameText;

    private Camera mainCam;
    private CardGeneratorManager cardGeneratorManager;

    private void Awake()
    {
        cardGeneratorManager = CardGeneratorManager.Instance;
    }

    private void Start()
    {
        currentDecorationCard = null;
        if (decorationCardData != null ) 
        {
            decorationIcon.sprite = decorationCardData.UiSprite;
            decorationNameText.text = decorationCardData.CardName;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentDecorationCard != null)
        {
            return;
        }

        if (cardGeneratorManager !=  null) 
        {
            mainCam = Camera.main;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -mainCam.transform.position.z;
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

            CardController newCardController = cardGeneratorManager.GenerateCard(decorationCardData, mousePos);
            currentDecorationCard = newCardController as DecorationCardController;

            /*SpriteRenderer spriteRenderer = newDecorationCardObj.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "DraggedUi";
            }*/

            currentDecorationCard.StartDraggedByUi();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentDecorationCard != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -mainCam.transform.position.z;
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

            currentDecorationCard.transform.position = targetPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log($"{decorationCardData.CardName} DECOR UI OVERLAP WITH UI");
            currentDecorationCard.gameObject.SetActive(false);
        }
        else
        {
            currentDecorationCard.StopDraggedByUi();
        }

        currentDecorationCard = null;
    }
}
