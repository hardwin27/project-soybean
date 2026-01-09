using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckCardController : CardController, ICardTaker
{
    [SerializeField] protected DeckCardData deckCardData;
    [SerializeField] protected int currentCardIndex;

    public Action OnDeckCardUpdated;
    public Action<CardData> OnDeckCardGenerated;

    public DeckCardData DeckCardData { get => deckCardData;  }
    public CardOnDeckData CurrentCardOnDeck
    {
        get
        {
            if (deckCardData == null) 
            {
                return null;
            }

            return deckCardData.CardsOnDeck[currentCardIndex] ?? null;
        }
    }


    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void OnMouseUp()
    {
        AudioManager.Instance.PlaySFXObject("card_dragged");
        return;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        SetPos(lastPost);
        ToggleDragSorting(false, ZOrder);
    }

    public override void AssignCardData(CardData data)
    {
        base.AssignCardData(data);
        deckCardData = data as DeckCardData;
        canBeDragged = false;

        OnDeckCardUpdated?.Invoke();
    }

    public virtual bool CanTakeCard(List<CardController> cardStacks)
    {
        if (deckCardData == null) 
            return false;

        return false;
    }

    public virtual void TakeCard(List<CardController> cardStacks)
    {
        if (deckCardData == null)
            return;
    }

    public void ChangeToNextCard()
    {
        currentCardIndex++;
        if (currentCardIndex >= deckCardData.CardsOnDeck.Count)
        {
            if (DeckCardData.IsLimited)
            {
                gameObject.SetActive(false);
                return;
            }
            else
            {
                currentCardIndex = 0;
            }
        }

        OnDeckCardUpdated?.Invoke();
    }
}
