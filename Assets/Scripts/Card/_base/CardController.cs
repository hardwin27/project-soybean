using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using ReadOnlyEditor;


// NOTE: TRY OUT THE GENERIC APPROACH IN THE FUTURE. IT'S ON DISCORD IMPORTANT
[RequireComponent(typeof(Collider2D), typeof(SortingGroup))]
public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected Collider2D cardCollider;
    protected SortingGroup cardSorting;

    [SerializeField] protected CardData cardData;
    [SerializeField] protected bool canBeDragged;
    [SerializeField, ReadOnly] protected bool isOnProcess = false;
    [SerializeField, ReadOnly] protected bool isTopStack = false;
    [SerializeField, ReadOnly] protected bool isDragged;

    [SerializeField] protected Transform stackPoint;

    [SerializeField] protected bool isAllowedToStack = true;
    [SerializeField, ReadOnly] protected CardController stackedOnCard;
    [SerializeField, ReadOnly] protected CardController topCard;
    [SerializeField, ReadOnly] protected Vector3 lastPost;
    [SerializeField, ReadOnly] protected Vector3 dragOffset;

    [SerializeField] protected string cardDefaultSortName;
    [SerializeField] protected string cardDraggedSortName;

    [SerializeField] private float cardWidth = 1f;
    [SerializeField] private float cardHeight = 1.4f;
    [SerializeField] private Rect cardBoundary;
    [SerializeField] private bool shouldBeBounded = true;


    [SerializeField, ReadOnly] List<CardController> overlapCardControllers = new List<CardController>();

    public Action OnBaseDataUpdated;

    public Action OnCardPosDragged;
    public Action OnCardUnstacked;
    public Action OnCardDestroyed;
    public Action<CardController> OnCardStacked;
    public Action OnCardDragEnd;
    public Action<bool, int> OnDragSorted;

    public CardData CardData { get => cardData;  }

    public bool CanBeDragged { get => canBeDragged; }
    public bool IsOnProcess { get => isOnProcess; set => isOnProcess = value; }
    public bool IsTopStack { get => isTopStack; set => isTopStack = value; }
    public bool IsDragged { get => isDragged; }

    public Transform StackPoint { get => stackPoint; }

    public bool IsStacked { get => stackedOnCard != null; }
    public CardController StackedOnCard { get => stackedOnCard; }
    public CardController TopCard { get => topCard; }

    public int ZOrder
    {
        protected set
        {
            if (cardSorting != null)
            {
                cardSorting.sortingOrder = value;
            }
            transform.position = new Vector3 (transform.position.x, transform.position.y, -value);
        }
        get
        {
            if (cardSorting != null)
            {
                return cardSorting.sortingOrder;
            }

            return 0;
        }
    }

    protected virtual void Awake()
    {
        cardCollider = GetComponent<Collider2D>();
        cardSorting = GetComponent<SortingGroup>();

        BoxCollider2D boxCol = cardCollider as BoxCollider2D;

        if (boxCol != null) 
        {
            cardWidth = boxCol.size.x;
            cardHeight = boxCol.size.y;
        }

        isDragged = false;
    }

    protected void Start()
    {
        AssignCardData(cardData);
        lastPost = transform.position;
        SetTopCard(this);
        /*HandleDragEnd();*/

        BoundCardPos(transform.position);
        StartCoroutine("DelayedOverlapCheck");
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"EventData: {eventData.pointerPressRaycast.gameObject.name}");
        Debug.Log($"EventData Postion {Camera.main.ScreenToWorldPoint(eventData.position)}");

        if (!CanBeDragged || IsOnProcess)
        { 
            return; 
        }

        dragOffset = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);

        AudioManager.Instance.PlaySFXObject("card_dragged");

        lastPost = transform.position;

        transform.SetAsFirstSibling();

        SetDragPos(eventData);
        ToggleDragSorting(true, ZOrder);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!CanBeDragged || IsOnProcess)
        {
            return;
        }

        /*Debug.Log($"Dragging {gameObject.name}");*/

        SetDragPos(eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!CanBeDragged || IsOnProcess)
        {
            return;
        }

        HandleDragEnd();
    }

    public void OnDisable()
    {
        Debug.Log($"{gameObject.name} DESTROYED");
        OnCardDestroyed?.Invoke();
        if (StackedOnCard != null)
        {
            stackedOnCard.SetTopCard(stackedOnCard);

            //Separated Logic
            StackedOnCard.OnCardPosDragged -= HandleStackedPosDragged;
            StackedOnCard.OnDragSorted -= HandleStackedDragSorted;
            StackedOnCard.OnCardDestroyed -= HandleStackedCardDestroyed;
            StackedOnCard.OnCardUnstacked?.Invoke();
            stackedOnCard = null;

        }
        Destroy(gameObject, 3f);
    }

    private System.Collections.IEnumerator DelayedOverlapCheck()
    {
        yield return null;

        Debug.Log($"DelayedOverlapCheck {gameObject.name}");

        /*int originalZ = ZOrder;
        ZOrder = 999999;*/

        yield return null;

        List<Collider2D> overlapColliders = new List<Collider2D>();
        Physics2D.OverlapCollider(cardCollider, new ContactFilter2D().NoFilter(), overlapColliders);

        /*ZOrder = originalZ;*/

        overlapCardControllers = new List<CardController>();
        foreach (Collider2D overlapCollider in overlapColliders)
        {
            if (overlapCollider.TryGetComponent(out CardController cardController))
            {
                if (!cardController.IsDragged)
                {
                    overlapCardControllers.Add(cardController);
                    Debug.Log($"DelayedOverlapCheck {gameObject.name} overlap with {cardController.gameObject.name}");
                    if (ZOrder <= cardController.ZOrder)
                    {
                        ZOrder = cardController.ZOrder + 1;
                    }
                }
            }
        }
    }

    public void SetTopCard(CardController card)
    {
        Debug.Log($"{gameObject.name} assign {card.gameObject.name} as TOPCARD");
        topCard = card;
        if (topCard == null)
        {
            topCard = this;
        }
        if (stackedOnCard != null && stackedOnCard != this && stackedOnCard != card)
        {
            stackedOnCard.SetTopCard(card);
        }
    }

    protected void HandleStackedCardDestroyed()
    {
        StackedOnCard.OnCardPosDragged -= HandleStackedPosDragged;
        StackedOnCard.OnDragSorted -= HandleStackedDragSorted;
        StackedOnCard.OnCardDestroyed -= HandleStackedCardDestroyed;
        StackedOnCard.OnCardUnstacked?.Invoke();
        stackedOnCard = null;
    }

    public virtual void AssignCardData(CardData data)
    {
        cardData = data;
        OnBaseDataUpdated?.Invoke();
    }

    protected void SetDragPos(PointerEventData eventData)
    {
        BoundCardPos(Camera.main.ScreenToWorldPoint(eventData.position));

        /*Vector3 dragPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector3 targetPos = new Vector3(dragPos.x + dragOffset.x, dragPos.y + dragOffset.y, transform.position.z);

        // Clamp the position so the card’s edges stay inside the boundary
        float halfW = cardWidth * 0.5f;
        float halfH = cardHeight * 0.5f;

        float minX = cardBoundary.xMin + halfW;
        float maxX = cardBoundary.xMax - halfW;
        float minY = cardBoundary.yMin + halfH;
        float maxY = cardBoundary.yMax - halfH;

        float clampedX = Mathf.Clamp(targetPos.x, minX + halfW, maxX - halfW);
        float clampedY = Mathf.Clamp(targetPos.y, minY + halfH, maxY - halfH);

        transform.position = new Vector3(clampedX, clampedY, targetPos.z);*/
        OnCardPosDragged?.Invoke();
    }

    protected void BoundCardPos(Vector3 posToBound)
    {
        Vector3 targetPos = new Vector3(posToBound.x + dragOffset.x, posToBound.y + dragOffset.y, transform.position.z);

        if (shouldBeBounded)
        {
            // Clamp the position so the card’s edges stay inside the boundary
            float halfW = cardWidth * 0.5f;
            float halfH = cardHeight * 0.5f;

            float minX = cardBoundary.xMin + halfW;
            float maxX = cardBoundary.xMax - halfW;
            float minY = cardBoundary.yMin + halfH;
            float maxY = cardBoundary.yMax - halfH;

            float clampedX = Mathf.Clamp(targetPos.x, minX + halfW, maxX - halfW);
            float clampedY = Mathf.Clamp(targetPos.y, minY + halfH, maxY - halfH);

            transform.position = new Vector3(clampedX, clampedY, targetPos.z);
        }

        else
        {
            transform.position = targetPos;
        }
    }

    protected void SetPos(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        lastPost = transform.position;
        OnCardPosDragged?.Invoke();
    }

    protected void ToggleDragSorting(bool isDrag, int sortingOrder = 0)
    {
        isDragged = isDrag;
        cardSorting.sortingLayerName = (isDragged) ? cardDraggedSortName : cardDefaultSortName;
        ZOrder = sortingOrder;
        OnDragSorted?.Invoke(isDragged, sortingOrder);
    }

    protected void HandleStackedPosDragged()
    {
        if (StackedOnCard == null && StackedOnCard != this)
        {
            return;
        }

        Debug.Log($"HandleStackedPosDragged {gameObject.name}");
        transform.position = new Vector3(StackedOnCard.StackPoint.position.x, StackedOnCard.StackPoint.position.y, transform.position.z);
        OnCardPosDragged?.Invoke();
    }

    protected void HandleStackedDragSorted(bool isDragged, int sortingOrder)
    {
        if (StackedOnCard == null)
        {
            return;
        }

        ToggleDragSorting(isDragged, sortingOrder + 1);
        /*HandleCardStackPos();*/
    }

    public void StackWithCard(CardController cardToStackTo)
    {
        if (cardToStackTo == this)
        {
            return;
        }

        if (StackedOnCard != null)
        {
            if (cardToStackTo == StackedOnCard)
            {
                return;
            }

            StackedOnCard.OnCardPosDragged -= HandleStackedPosDragged;
            StackedOnCard.OnDragSorted -= HandleStackedDragSorted;
            StackedOnCard.OnCardDestroyed -= HandleStackedCardDestroyed;
            StackedOnCard.OnCardUnstacked?.Invoke();
            stackedOnCard.SetTopCard(stackedOnCard);
            stackedOnCard = null;
        }

        if (cardToStackTo == null)
        {
            ToggleDragSorting(false);
            lastPost = transform.position;
        }
        else
        {
            stackedOnCard = cardToStackTo;
            stackedOnCard.SetTopCard(TopCard);
            StackedOnCard.OnCardPosDragged += HandleStackedPosDragged;
            StackedOnCard.OnDragSorted += HandleStackedDragSorted;
            StackedOnCard.OnCardDestroyed += HandleStackedCardDestroyed;

            SetPos(StackedOnCard.StackPoint.position);
            ToggleDragSorting(false, cardToStackTo.ZOrder + 1);
            OnCardStacked?.Invoke(this);
        }
    }

    protected bool CanStackWithCard(CardController cardController)
    {
        if (cardController == this)
        {
            return false;
        }

        if (cardController == TopCard)
        {
            return false;
        }

        if (StackedOnCard == null)
        {
            return true;
        }
        else
        {
            CardController parentCard = StackedOnCard;
            while (parentCard != null) 
            {
                if (parentCard == cardController)
                {
                    return false;
                }
                else
                {
                    parentCard = parentCard.StackedOnCard;
                }
            }

            return true;
        }
    }

    protected void HandleDragEnd()
    {
        List<Collider2D> overlapColliders = new List<Collider2D>();
        Physics2D.OverlapCollider(cardCollider, new ContactFilter2D().NoFilter(), overlapColliders);

        overlapCardControllers = new List<CardController>();
        foreach (Collider2D overlapCollider in overlapColliders)
        {
            if (overlapCollider.TryGetComponent(out CardController cardController))
            {
                if (!cardController.IsDragged)
                {
                    overlapCardControllers.Add(cardController);
                }
            }
        }

        if (isAllowedToStack)
        {
            if (overlapCardControllers.Count <= 0)
            {
                ToggleDragSorting(false);
                StackWithCard(null);
            }
            else
            {
                overlapCardControllers.Sort((a, b) =>
                {
                    float distA = Vector3.SqrMagnitude(a.transform.position - transform.position);
                    float distB = Vector3.SqrMagnitude(b.transform.position - transform.position);
                    return distA.CompareTo(distB);
                });

                foreach (CardController cardController in overlapCardControllers)
                {
                    if (CanStackWithCard(cardController.TopCard))
                    {
                        Debug.Log($"Stack To Card: {cardController.TopCard}");
                        StackWithCard(cardController.TopCard);
                        return;
                    }
                }

                Debug.LogWarning($"Rejected Form Stack");
                SetPos(lastPost);
                ToggleDragSorting(false, ZOrder);
            }
        }
        else
        {
            if (overlapCardControllers.Count <= 0)
            {
                ToggleDragSorting(false);
                StackWithCard(null);
            }
            else
            {
                Debug.LogWarning($"Rejected Form Stack");
                SetPos(lastPost);
                ToggleDragSorting(false, ZOrder);
            }
        }
    }
}
