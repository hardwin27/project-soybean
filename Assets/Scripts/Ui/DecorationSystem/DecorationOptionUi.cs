using UnityEngine;
using UnityEngine.UI;
using ReadOnlyEditor;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngineInternal;

public class DecorationOptionUi : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField, ReadOnly] private DecorationListingData decorationListingData;
    [SerializeField, ReadOnly] private DecorationCardController currentDecorationCard;

    [SerializeField] private Image decorationIcon;
    [SerializeField] private TextMeshProUGUI decorationNameText;
    [SerializeField] private GameObject lockedPanel;
    [SerializeField] private TextMeshProUGUI lockedDescText;

    private Camera mainCam;
    private CardGeneratorManager cardGeneratorManager;

    private void Awake()
    {
        cardGeneratorManager = CardGeneratorManager.Instance;
    }

    private void Start()
    {
        currentDecorationCard = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentDecorationCard != null)
        {
            return;
        }

        if (decorationListingData == null)
        {
            return;
        }

        if (!decorationListingData.IsUnlocked)
        {
            return;
        }

        if (decorationListingData.DecorationCardData == null)
        {
            return;
        }

        if (cardGeneratorManager !=  null) 
        {
            mainCam = Camera.main;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -mainCam.transform.position.z;
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

            CardController newCardController = cardGeneratorManager.GenerateCard(decorationListingData.DecorationCardData, targetPos);
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
            /*Debug.Log($"{decorationListingData.DecorationCardData.CardName} DECOR UI OVERLAP WITH UI");*/
            currentDecorationCard.gameObject.SetActive(false);
        }
        else
        {
            currentDecorationCard.StopDraggedByUi();
        }

        currentDecorationCard = null;
    }

    public void AssignDecorationListing(DecorationListingData _decorationListingData)
    {
        if (_decorationListingData.DecorationCardData != null) 
        {
            decorationListingData = _decorationListingData;

            decorationIcon.sprite = decorationListingData.DecorationCardData.UiSprite;
            decorationNameText.text = decorationListingData.DecorationCardData.CardName;
            UpdateLockedPanel();

            decorationListingData.OnIsUnlockedUpdated += UpdateLockedPanel;
        } 
    }

    private void UpdateLockedPanel()
    {
        lockedPanel.SetActive(!decorationListingData.IsUnlocked);
    }
}
