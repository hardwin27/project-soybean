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
    [SerializeField, ReadOnly] protected CardController topCard;
    [SerializeField, ReadOnly] protected Vector3 lastPost;

    [SerializeField] protected string cardDefaultSortName;
    [SerializeField] protected string cardDraggedSortName;

    [Title("Debugging")]
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

        isDragged = false;
    }

    protected void Start()
    {
        AssignCardData(cardData);
        lastPost = transform.position;
        SetTopCard(this);
        /*HandleDragEnd();*/
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanBeDragged || IsOnProcess)
        { 
            return; 
        }

        AudioManager.Instance.PlaySFXObject("card_dragged");

        lastPost = transform.position;

        SetDragPos(eventData);
        ToggleDragSorting(true, ZOrder);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanBeDragged || IsOnProcess)
        {
            return;
        }

        Debug.Log($"Dragging {gameObject.name}");

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

    public void SetTopCard(CardController card)
    {
        topCard = card;
    }

    protected void HandleStackedCardDestroyed()
    {
        StackedOnCard.OnCardPosDragged -= HandleStackedPosDragged;
        StackedOnCard.OnDragSorted -= HandleStackedDragSorted;
        StackedOnCard.OnCardDestroyed -= HandleStackedCardDestroyed;
        StackedOnCard.OnCardUnstacked?.Invoke();
        stackedOnCard.SetTopCard(stackedOnCard);
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
}
