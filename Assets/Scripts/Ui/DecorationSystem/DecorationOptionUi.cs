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
    [SerializeField] private GameObject decorationCounterPanel;
    [SerializeField] private TextMeshProUGUI decorationQtyText;
    [SerializeField] private GameObject lockedPanel;
    [SerializeField] private TextMeshProUGUI lockedDescText;

    private Camera mainCam;
    private DecorationManager decorationManager;

    private void Awake()
    {
        decorationManager = DecorationManager.Instance;
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

        if (decorationListingData.StockAmount <= 0)
        {
            return;
        }

        if (decorationListingData.DecorationCardData == null)
        {
            return;
        }

        if (decorationManager !=  null) 
        {
            mainCam = Camera.main;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -mainCam.transform.position.z;
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

            currentDecorationCard = decorationManager.GenerateDecorationCard(decorationListingData, targetPos);

            /*SpriteRenderer spriteRenderer = newDecorationCardObj.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "DraggedUi";
            }*/

            currentDecorationCard.StartDraggedByUi();

            AudioManager.Instance?.PlayDefaultUiClick();
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
        if (currentDecorationCard != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                /*Debug.Log($"{decorationListingData.DecorationCardData.CardName} DECOR UI OVERLAP WITH UI");*/
                currentDecorationCard.gameObject.SetActive(false);
            }
            else
            {
                currentDecorationCard.StopDraggedByUi();
                AudioManager.Instance?.PlaySFXObject("decoration_on_placed");
            }

            currentDecorationCard = null;
        }
    }

    public void AssignDecorationListing(DecorationListingData _decorationListingData)
    {
        if (_decorationListingData.DecorationCardData != null) 
        {
            decorationListingData = _decorationListingData;

            decorationIcon.sprite = decorationListingData.DecorationCardData.UiSprite;
            decorationNameText.text = decorationListingData.DecorationCardData.CardName;
            UpdateLockedPanel();
            UpdateQtyText();

            decorationListingData.OnIsUnlockedUpdated += UpdateLockedPanel;
            decorationListingData.OnStockUpdated += UpdateQtyText;
        } 
    }

    private void UpdateLockedPanel()
    {
        lockedPanel.SetActive(!decorationListingData.IsUnlocked);
        decorationCounterPanel.SetActive(decorationListingData.IsUnlocked);
    }

    private void UpdateQtyText()
    {
        decorationQtyText.text = decorationListingData.StockAmount.ToString();
    }
}
