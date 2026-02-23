using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using ReadOnlyEditor;
using NUnit.Framework.Constraints;

[RequireComponent(typeof(Collider2D), typeof(SortingGroup))]
public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected Collider2D cardCollider;
    protected SortingGroup cardSorting;

    [Header("Data")]
    [SerializeField] protected CardData cardData;

    [Header("State")]
    [SerializeField] protected bool canBeDragged = true;
    [SerializeField, ReadOnly] protected bool isOnProcess = false;
    [SerializeField, ReadOnly] protected bool isTopStack = false;
    [SerializeField, ReadOnly] protected bool isDragged;

    [Header("Stacking")]
    [SerializeField] protected Transform stackPoint;
    [SerializeField] protected bool isAllowedToStack = true;
    [SerializeField, ReadOnly] protected CardController stackedOnCard;
    [SerializeField, ReadOnly] protected CardController topCard;

    [Header("Movement & Boundary")]
    [SerializeField, ReadOnly] protected Vector3 lastPost;
    [SerializeField, ReadOnly] protected Vector3 dragOffset;

    [Tooltip("Boundary")]
    [SerializeField, ReadOnly] protected Collider2D playAreaBoundary;
    [SerializeField] private bool shouldBeBounded = true;

    [Header("Visuals")]
    [SerializeField] protected string cardDefaultSortName;
    [SerializeField] protected string cardDraggedSortName;
    [SerializeField] protected bool isInitiallyFlipped;

    [Header("Audio")]
    [SerializeField] protected string cardDraggedAudioCode;
    [SerializeField] protected string cardDroppedAudioCode;

    [SerializeField, ReadOnly] List<CardController> overlapCardControllers = new List<CardController>();

    public Action OnBaseDataUpdated;
    public Action OnCardPosDragged;
    public Action OnCardUnstacked;
    public Action OnCardDestroyed;
    public Action<CardController> OnCardStacked;
    public Action OnCardDragEnd;
    public Action<bool, int> OnDragSorted;
    public Action<bool> OnHoverToggled;

    public CardData CardData { get => cardData; }
    public bool CanBeDragged { get => canBeDragged; }
    public bool IsOnProcess { get => isOnProcess; set => isOnProcess = value; }
    public bool IsTopStack { get => (TopCard == this); }
    public bool IsDragged { get => isDragged; }
    public Transform StackPoint { get => stackPoint; }
    public bool IsStacked { get => stackedOnCard != null; }
    public CardController StackedOnCard { get => stackedOnCard; }
    public CardController TopCard { get => topCard; }

    public int ZOrder
    {
        protected set
        {
            if (cardSorting != null) cardSorting.sortingOrder = value;
            transform.position = new Vector3(transform.position.x, transform.position.y, -value);
        }
        get => cardSorting != null ? cardSorting.sortingOrder : 0;
    }

    protected virtual void Awake()
    {
        cardCollider = GetComponent<Collider2D>();
        cardSorting = GetComponent<SortingGroup>();
        isDragged = false;
    }

    protected void Start()
    {
        AssignCardData(cardData);
        lastPost = transform.position;
        SetTopCard(this);
        RequestHoverToggle(false);
        if (isInitiallyFlipped)
        {
            FlipCollider();
        }

        BoundCardPos(transform.position);
        StartCoroutine(DelayedOverlapCheck());
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanBeDragged || IsOnProcess) return;

        AudioManager.Instance.PlaySFXObject(cardDraggedAudioCode);

        dragOffset = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);

        lastPost = transform.position;
        transform.SetAsFirstSibling();

        SetDragPos(eventData);
        ToggleDragSorting(true, ZOrder);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!CanBeDragged || IsOnProcess) return;
        SetDragPos(eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!CanBeDragged || IsOnProcess) return;
        HandleDragEnd();
        AudioManager.Instance.PlaySFXObject(cardDroppedAudioCode);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsTopStack)
        {
            RequestHoverToggle(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RequestHoverToggle(false);
    }

    public void OnDisable()
    {
        OnCardDestroyed?.Invoke();
        if (StackedOnCard != null)
        {
            stackedOnCard.SetTopCard(stackedOnCard);
            StackedOnCard.OnCardPosDragged -= HandleStackedPosDragged;
            StackedOnCard.OnDragSorted -= HandleStackedDragSorted;
            StackedOnCard.OnCardDestroyed -= HandleStackedCardDestroyed;
            StackedOnCard.OnCardUnstacked?.Invoke();
            stackedOnCard = null;
        }
        Destroy(gameObject, 3f);
    }

    public void FlipCollider()
    {
        if (cardCollider is PolygonCollider2D poly)
        {
            List<Vector2> currentPoints = new List<Vector2>();

            int pathCount = poly.GetPath(0, currentPoints);

            for (int i = 0; i < currentPoints.Count; i++)
            {
                float x = Mathf.Abs(currentPoints[i].x);
                Vector2 newPoint = currentPoints[i];
                newPoint.x = -1f * newPoint.x;
                currentPoints[i] = newPoint;
            }

            // 3. Set the path back
            poly.SetPath(0, currentPoints);
        }
    }

    public void RequestHoverToggle(bool isHovered)
    {
        OnHoverToggled?.Invoke(isHovered);
    }

    protected System.Collections.IEnumerator DelayedOverlapCheck()
    {
        yield return new WaitForFixedUpdate();
        /*List<Collider2D> overlapColliders = new List<Collider2D>();
        Physics2D.OverlapCollider(cardCollider, new ContactFilter2D().NoFilter(), overlapColliders);

        overlapCardControllers = new List<CardController>();
        foreach (Collider2D overlapCollider in overlapColliders)
        {
            if (overlapCollider.TryGetComponent(out CardController cardController))
            {
                if (!cardController.IsDragged)
                {
                    overlapCardControllers.Add(cardController);
                    if (ZOrder <= cardController.ZOrder)
                    {
                        ZOrder = cardController.ZOrder + 1;
                    }
                }
            }
        }*/

        HandleDragEnd(ignoreBuilding: true);
    }

    public void SetTopCard(CardController card)
    {
        topCard = card ?? this;
        if (stackedOnCard != null && stackedOnCard != this && stackedOnCard != card)
        {
            stackedOnCard.SetTopCard(card);
        }
    }

    protected void HandleStackedCardDestroyed()
    {
        if (StackedOnCard == null) return;

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
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        BoundCardPos(worldPoint);
        OnCardPosDragged?.Invoke();
    }

    public void AssignBoundary(Collider2D boundaryCol)
    {
        playAreaBoundary = boundaryCol;
    }

    protected void BoundCardPos(Vector3 posToBound)
    {
        Vector3 targetPos = new Vector3(posToBound.x + dragOffset.x, posToBound.y + dragOffset.y, transform.position.z);

        if (shouldBeBounded && playAreaBoundary != null)
        {
            Bounds areaBounds = playAreaBoundary.bounds;

            Vector3 cardHalfSize = cardCollider.bounds.extents;

            float minX = areaBounds.min.x + cardHalfSize.x;
            float maxX = areaBounds.max.x - cardHalfSize.x;
            float minY = areaBounds.min.y + cardHalfSize.y;
            float maxY = areaBounds.max.y - cardHalfSize.y;

            float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
            float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

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
        if (cardSorting != null)
        {
            cardSorting.sortingLayerName = (isDragged) ? cardDraggedSortName : cardDefaultSortName;
        }
        ZOrder = sortingOrder;
        OnDragSorted?.Invoke(isDragged, sortingOrder);
    }

    protected void HandleStackedPosDragged()
    {
        if (StackedOnCard == null && StackedOnCard != this) return;

        transform.position = new Vector3(StackedOnCard.StackPoint.position.x, StackedOnCard.StackPoint.position.y, transform.position.z);
        OnCardPosDragged?.Invoke();
    }

    protected void HandleStackedDragSorted(bool isDragged, int sortingOrder)
    {
        if (StackedOnCard == null) return;
        ToggleDragSorting(isDragged, sortingOrder + 1);
    }

    public void StackWithCard(CardController cardToStackTo)
    {
        if (cardToStackTo == this) return;

        if (StackedOnCard != null)
        {
            if (cardToStackTo == StackedOnCard) return;

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
        if (cardController == this || cardController == TopCard) return false;

        if (StackedOnCard == null) return true;

        CardController parentCard = StackedOnCard;
        while (parentCard != null)
        {
            if (parentCard == cardController) return false;
            parentCard = parentCard.StackedOnCard;
        }
        return true;
    }

    protected void HandleDragEnd(bool ignoreBuilding = false)
    {
        List<Collider2D> overlapColliders = new List<Collider2D>();
        Physics2D.OverlapCollider(cardCollider, ContactFilter2D.noFilter, overlapColliders);

        overlapCardControllers = new List<CardController>();
        foreach (Collider2D overlapCollider in overlapColliders)
        {
            Debug.Log($"{gameObject.name} overlap with {overlapCollider.gameObject.name}");
            if (overlapCollider.TryGetComponent(out CardController cardController))
            {
                if (ignoreBuilding)
                {
                    if (cardController is BuildingCardController)
                    {
                        continue;
                    }
                }
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
                        StackWithCard(cardController.TopCard);
                        return;
                    }
                }

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
                SetPos(lastPost);
                ToggleDragSorting(false, ZOrder);
            }
        }
    }

    public List<CardController> GetStackData()
    {
        List<CardController> stack = new List<CardController>();

        CardController cardOnStack = TopCard;
        while (cardOnStack != null) 
        {
            stack.Add(cardOnStack);
            cardOnStack = cardOnStack.StackedOnCard;
        }

        return stack;
    }
}