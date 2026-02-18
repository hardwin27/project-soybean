using UnityEngine;
using TMPro;
using ReadOnlyEditor;
using System.Collections.Generic;

[System.Serializable]
public class HoverUiData
{
    [SerializeField, ReadOnly] private CardData cardData;
    [SerializeField, ReadOnly] private int count;

    public CardData CardData { get => cardData; }
    public int Count { get => count; }

    public HoverUiData(CardData _cardData)
    {
        cardData = _cardData;
        count = 0;
    }

    public void AddCount(int addedCount)
    {
        count += addedCount;
    }
}

[RequireComponent(typeof(CardController))]
public class CardVisual : MonoBehaviour
{
    /*[System.Serializable] 
    protected class CardTypeColor
    {
        public CardType CardType;
        public Color Color;
    }*/

    protected CardController cardController;

    [SerializeField] protected GameObject visualParent;
    [SerializeField] protected bool isReqSpriteAssg = true;
    [SerializeField] protected TextMeshPro cardNameText;
    [SerializeField] protected SpriteRenderer cardRenderer;
    [SerializeField] protected Vector3 cardRendererSpriteScale;
    /*[SerializeField] protected List<CardTypeColor> cardTypeColors;*/
    /*[SerializeField] protected SimpleTooltip cardToolTip;*/
    [Header("Hover Ui")]
    [SerializeField] protected GameObject hoverUiParent;
    [SerializeField] protected TextMeshProUGUI hoverUiTextTemplate;
    [SerializeField, ReadOnly] protected List<HoverUiData> hoverUiDatas = new List<HoverUiData>();
    [SerializeField, ReadOnly] protected List<TextMeshProUGUI> hoverUiTexts = new List<TextMeshProUGUI>();
    protected float closeHoverDelay = 0.1f;
    protected Coroutine closeHoverCoroutine;

    protected virtual void Awake()
    {
        cardController = GetComponent<CardController>();
        cardController.OnBaseDataUpdated += UpdateBaseVisual;
        cardController.OnHoverToggled += ToggleHoverUi;
    }

    private void Start()
    {
        ToggleVisibility(true);
    }

    public void ToggleHoverUi(bool showHoverUi)
    {
        if (hoverUiParent != null) 
        {
            if (showHoverUi)
            {
                if (closeHoverCoroutine != null) 
                {
                    StopCoroutine(closeHoverCoroutine);
                    closeHoverCoroutine = null;
                }

                hoverUiParent.SetActive(true);

                UpdateHoverDisplay();
            }
            else
            {
                closeHoverCoroutine = StartCoroutine(DelayedCloseHover());
            }
        }
    }

    protected virtual void UpdateHoverDisplay()
    {
        foreach (var hoverUiText in hoverUiTexts)
        {
            hoverUiText.gameObject.SetActive(false);
        }

        hoverUiDatas.Clear();

        foreach (var card in cardController.GetStackData())
        {
            HoverUiData hoverUiData = hoverUiDatas.Find(data => data.CardData == card.CardData);
            if (hoverUiData == null)
            {
                hoverUiData = new HoverUiData(card.CardData);
                hoverUiDatas.Add(hoverUiData);
            }
            hoverUiData.AddCount(1);
        }

        int difference = hoverUiDatas.Count - hoverUiTexts.Count;
        while (difference-- > 0)
        {
            TextMeshProUGUI newText = Instantiate(hoverUiTextTemplate.gameObject,
                hoverUiParent.transform).GetComponent<TextMeshProUGUI>();
            hoverUiTexts.Add(newText);
            newText.gameObject.SetActive(false);
        }

        for (int x = 0; x < hoverUiDatas.Count; x++)
        {
            hoverUiTexts[x].gameObject.SetActive(true);
            hoverUiTexts[x].text = $"{hoverUiDatas[x].CardData.CardName}";
            if (hoverUiDatas[x].Count > 1)
            {
                hoverUiTexts[x].text += $" ({hoverUiDatas[x].Count})";
            }
        }

        /*if (hoverUiDatas.Count == 1) 
        {
            hoverUiTexts[0].gameObject.SetActive(true);
            hoverUiTexts[0].text = $"{hoverUiDatas[0].CardData.CardName}";
        }
        else
        {
            for (int x = 0; x < hoverUiDatas.Count; x++)
            {
                hoverUiTexts[x].gameObject.SetActive(true);
                hoverUiTexts[x].text = $"{hoverUiDatas[x].CardData.CardName} ({hoverUiDatas[x].Count})";
            }
        }*/
    }

    protected System.Collections.IEnumerator DelayedCloseHover()
    {
        yield return new WaitForSeconds(closeHoverDelay);
        hoverUiParent.SetActive(false);
    }

    public void ToggleVisibility(bool isVisible)
    {
        visualParent.SetActive(isVisible);
    }

    public void TintVisual(Color tintColor)
    {
        cardRenderer.color = tintColor;
    }

    protected virtual void UpdateBaseVisual()
    {
        CardData data = cardController.CardData;

        if (data != null)
        {
            gameObject.name = data.CardName;
            cardNameText.text = data.CardName;

            UpdateBaseSprite(data.CardSprite, data.CardType);   
        }
    }

    protected void DebugUpdateBaseSprite()
    {
        cardController = GetComponent<CardController>();
        if (cardController != null)
        {
            CardData cardData = cardController.CardData;
            if (cardData != null)
            {
                UpdateBaseSprite(cardData.CardSprite, cardData.CardType);
            }
        }
    }
    
    protected void UpdateBaseSprite(Sprite cardSprite, CardType cardType)
    {
        if (!isReqSpriteAssg)
        {
            return;
        }

        if (cardSprite != null)
        {
            cardRenderer.sprite = cardSprite;
            cardRenderer.transform.localScale = /*new Vector3(0.3f, 0.3f, 0.3f)*/cardRendererSpriteScale;
            cardNameText.gameObject.SetActive(false);
        }
        /*else
        {
            foreach (var cardTypeColor in cardTypeColors)
            {
                if (cardTypeColor.CardType == cardType)
                {
                    cardRenderer.color = cardTypeColor.Color;
                    break;
                }
            }
        }*/

        /*cardToolTip.iconSprite = cardSprite;*/
    }
}
