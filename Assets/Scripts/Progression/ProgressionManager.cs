using System.Collections.Generic;
using UnityEngine;
using SingletonSystem;
using ReadOnlyEditor;

[System.Serializable]
public class ProductListing
{
    [SerializeField, ReadOnly] private CardData cardData;
    [SerializeField, ReadOnly] private int producedQty;
    [SerializeField, ReadOnly] private int sellQty;

    public CardData CardData { get => cardData; }
    public int ProducedQty { get => producedQty; set => producedQty = value; }
    public int SellQty { get => sellQty; set => sellQty = value; }

    public ProductListing(CardData _cardData)
    {
        this.cardData = _cardData;
        producedQty = 0;
        sellQty = 0;
    }
}

[System.Serializable]
public class ProgressionData
{
    [SerializeField, ReadOnly] private List<ProductListing> productListings;
    [SerializeField, ReadOnly] private int moneyCollected;
    [SerializeField, ReadOnly] private int moneySpent;
    [SerializeField, ReadOnly] private int ownedMoney;

    public List<ProductListing> ProductListings { get => productListings; }
    public int MoneyCollected { get => moneyCollected; set => moneyCollected = value; }
    public int MoneySpent { get => moneySpent; set => moneySpent = value; }
    public int OwnedMoney { get => ownedMoney; set => ownedMoney = value; }

    public ProgressionData()
    {
        productListings = new List<ProductListing>();
    }

    public void AddProductToListing(CardData cardProductData, int producedQty = 0, int sellQty = 0)
    {
        ProductListing productListing = productListings.Find(p => p.CardData ==  cardProductData);
        if (productListing == null) 
        {
            productListing = new ProductListing(cardProductData);
            productListings.Add(productListing);
        }

        productListing.ProducedQty += producedQty;
        productListing.SellQty += sellQty;
    }

    public void AddProducedProduct(CardData addedProduct, int producedAmount)
    {
        AddProductToListing(addedProduct, producedQty: producedAmount);
    }

    public void AddSoldProduct(CardData addedProduct, int soldAmount)
    {
        AddProductToListing(addedProduct, sellQty: soldAmount);
    }

    public void AddProgressionData(ProgressionData addedProgressionData)
    {
        foreach (var addedProdListing in addedProgressionData.ProductListings)
        {
            AddProductToListing(addedProdListing.CardData, 
                addedProdListing.ProducedQty, 
                addedProdListing.SellQty); 
        }

        MoneyCollected += addedProgressionData.moneyCollected;
        MoneySpent += addedProgressionData.MoneySpent;
        OwnedMoney += addedProgressionData.OwnedMoney;
    }
}

public class ProgressionManager : Singleton<ProgressionManager>
{
    [SerializeField, ReadOnly] private BuildingDeckSellCardController regiesteredMarket;
    [SerializeField, ReadOnly] private List<BuildingDeckBuyCardController> registeredShops;

    [Header("Progression Data")]
    [SerializeField, ReadOnly] private ProgressionData currentProgressionData;
    [SerializeField, ReadOnly] private ProgressionData prevProgressionData;
    [SerializeField, ReadOnly] private ProgressionData currentBatchProgressionData;
    [SerializeField, ReadOnly] private ProgressionData prevBatchProgressionData;
    
    private CardGeneratorManager cardGeneratorManager;

    private void Awake()
    {
        cardGeneratorManager = CardGeneratorManager.Instance;

        cardGeneratorManager.OnCardGenerated += HandleCardGenerated;
    }

    private void Start()
    {
        currentProgressionData = new ProgressionData();
        prevProgressionData = new ProgressionData();
        currentBatchProgressionData = new ProgressionData();
        prevBatchProgressionData = new ProgressionData();

        ScanPreviousMoney();
    }

    private void ScanPreviousMoney()
    {
        List<CardController> cardControllers = new List<CardController>(
            Object.FindObjectsByType<CardController>
            (FindObjectsInactive.Exclude, FindObjectsSortMode.None));

        foreach (var cardController in cardControllers) 
        {
            if (cardController.CardData.CardType == CardType.Money)
            {
                MoneyCardData moneyCardData = cardController.CardData as MoneyCardData;
                prevProgressionData.OwnedMoney += moneyCardData.MoneyValue;
            }
        }

        prevBatchProgressionData.OwnedMoney += prevProgressionData.OwnedMoney;
    }

    public void RegisterMarket(BuildingDeckSellCardController market)
    {
        if (regiesteredMarket != null)
        {
            regiesteredMarket.OnCardSold -= HandleCardSold;
            registeredShops = null;
        }

        regiesteredMarket = market;
        regiesteredMarket.OnCardSold += HandleCardSold;
    }

    public void RegisterShop(BuildingDeckBuyCardController shop)
    {
        if (!registeredShops.Contains(shop))
        { 
            registeredShops.Add(shop);
            shop.OnDeckCardGenerated += HandleCardBought;
        }
    }

    private void HandleCardSold(CardController cardControllerSold)
    {
        currentProgressionData.MoneyCollected += cardControllerSold.CardData.SellPrice;
        currentProgressionData.AddSoldProduct(cardControllerSold.CardData, 1);
    }

    private void HandleCardBought(CardData cardControllerBought)
    {
        currentProgressionData.MoneySpent += cardControllerBought.BuyPrice;
    }

    private void HandleCardGenerated(CardController cardController)
    {
        if (cardController.CardData.CardType == CardType.Product)
        {
            currentProgressionData.AddProducedProduct(cardController.CardData, 1);
        }
    }

    public void EndCurrentProgression()
    {
        currentProgressionData.OwnedMoney = 
            prevProgressionData.OwnedMoney + currentProgressionData.MoneyCollected - 
            currentProgressionData.MoneySpent;

        currentBatchProgressionData.AddProgressionData(currentProgressionData);
    }

    public void StartNextProgression()
    {
        prevProgressionData = currentProgressionData;
        currentProgressionData = new ProgressionData();
    }

    public void StartNextBatchProgression()
    {
        prevBatchProgressionData = currentBatchProgressionData;
        currentBatchProgressionData = new ProgressionData();
    }
}
