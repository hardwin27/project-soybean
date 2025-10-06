using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;



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

    [SerializeField, ReadOnly] protected CardController stackedOnCard;
    [SerializeField, ReadOnly] protected Vector3 lastPost;

    [SerializeField] protected string cardDefaultSortName;
    [SerializeField] protected string cardDraggedSortName;

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

    public int ZOrder
    {
        protected set
        {
            if (cardSorting != null)
            {
                cardSorting.sortingOrder = value;
            }
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

        if (cardData != null)
        {
            AssignCardData(cardData);
        }

        isDragged = false;
    }

    protected void Start()
    {
        lastPost = transform.position;
        HandleDragEnd();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanBeDragged || IsOnProcess)
        { 
            return; 
        }

        SetDragPos(eventData);
        ToggleDragSorting(true, 0);

        lastPost = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!!CanBeDragged || IsOnProcess)
        {
            return;
        }

        SetDragPos(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanBeDragged || IsOnProcess)
        {
            return;
        }

        HandleDragEnd();
    }

    public void OnDisable()
    {
        OnCardDestroyed?.Invoke();
        Destroy(gameObject, 3f);
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
        Vector3 dragPos = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.position = new Vector3(dragPos.x, dragPos.y, transform.position.z);
        OnCardPosDragged?.Invoke();
    }

    protected void SetPos(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
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
        if (StackedOnCard == null)
        {
            return;
        }

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

    protected void StackWithCard(CardController cardToStackTo)
    {
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
            stackedOnCard = null;
        }

        if (cardToStackTo == null)
        {
            ToggleDragSorting(false);
        }
        else
        {
            stackedOnCard = cardToStackTo;
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
        // for now prevent to stack on parents
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

        List<CardController> overlapCardController = new List<CardController>();
        foreach (Collider2D overlapCollider in overlapColliders)
        {
            if (overlapCollider.TryGetComponent(out CardController cardController))
            {
                if (!cardController.IsDragged)
                {
                    overlapCardController.Add(cardController);
                }
            }
        }

        if (overlapCardController.Count <= 0)
        {
            ToggleDragSorting(false);
            StackWithCard(null);
        }
        else
        {
            foreach (CardController cardController in overlapCardController)
            {
                /*StackWithCard(cardController);
                return;*/

                if (CanStackWithCard(cardController))
                {
                    StackWithCard(cardController);
                    return;
                }
            }

            transform.position = lastPost;
            ToggleDragSorting(false);
        }
    }
}
