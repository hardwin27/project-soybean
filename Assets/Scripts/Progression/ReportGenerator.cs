using UnityEngine;
using ReadOnlyEditor;
using System.Collections.Generic;
using System;

[System.Serializable]
public class ReportProductEntryData
{
    [SerializeField, ReadOnly] private CardData productCardData;
    [SerializeField, ReadOnly] private int producedQty;
    [SerializeField, ReadOnly] private QuantityComparisonType quantityComparisonType;
    [SerializeField, ReadOnly] private int sellQty;

    public CardData ProductCardData { get => productCardData; }
    public int ProducedQty { get =>  producedQty; }
    public QuantityComparisonType QuantityComparisonType { get => quantityComparisonType; }
    public int SellQty { get => sellQty; }

    public ReportProductEntryData(CardData _productCardData, int _produecdQty, QuantityComparisonType _quantityComparisonType, int _sellQty)
    {
        productCardData = _productCardData;
        producedQty = _produecdQty;
        quantityComparisonType = _quantityComparisonType; 
        sellQty = _sellQty;
    }
}

[System.Serializable]
public class ReportData
{
    [SerializeField, ReadOnly] private ReportType reportType;
    [SerializeField, ReadOnly] private string timeStamp;
    [SerializeField, ReadOnly] private List<ReportProductEntryData> productEntryDatas;
    [SerializeField, ReadOnly] private int moneyBefore;
    [SerializeField, ReadOnly] private int totalMoneyCollected;
    [SerializeField, ReadOnly] private int totalMoneySpent;
    [SerializeField, ReadOnly] private int remainingMoney;

    public ReportType ReportType { get => reportType; }
    public string TimeStamp { get => timeStamp; }
    public List<ReportProductEntryData> ReportProductEntryDatas { get => productEntryDatas; }
    public int MoneyBefore { get => moneyBefore; }
    public int TotalMoneyCollected { get => totalMoneyCollected; }
    public int TotalMoneySpent { get => totalMoneySpent; }
    public int RemainingMoney { get => remainingMoney; }

    public ReportData(ReportType _reportType, string _timestamp, List<ReportProductEntryData> _productEntryDatas, int _moneyBefore, int _totalMoneyCollected, int _totalMoneySpent, int _remainingMoney)
    {
        reportType = _reportType;
        timeStamp = _timestamp;
        productEntryDatas = _productEntryDatas;
        moneyBefore = _moneyBefore;
        totalMoneyCollected = _totalMoneyCollected;
        totalMoneySpent = _totalMoneySpent;
        remainingMoney = _remainingMoney;
    }
}

public class ReportGenerator : MonoBehaviour
{
    private GameTimeManager gameTimeManager;

    public Action<ReportData> OnReportGenerated;

    private void Awake()
    {
        gameTimeManager = GameTimeManager.Instance;
    }

    public void GenerateReport(ProgressionData prevProgression, ProgressionData currentProgression, ReportType reportType)
    {
        List<ReportProductEntryData> reportProductEntryDatas = new List<ReportProductEntryData>();

        foreach(var productListing in currentProgression.ProductListings)
        {
            QuantityComparisonType quantityComparisonType = QuantityComparisonType.None;

            ProductListing prevProductListing = prevProgression.ProductListings.Find(p => p.CardData == productListing.CardData);
            if (prevProductListing == null) 
            {
                if (productListing.ProducedQty > 0)
                {
                    quantityComparisonType = QuantityComparisonType.Increased;
                }
                else
                {
                    quantityComparisonType = QuantityComparisonType.None;
                }
            }
            else
            {
                if (productListing.ProducedQty > prevProductListing.ProducedQty)
                {
                    quantityComparisonType = QuantityComparisonType.Increased;
                }
                else if (productListing.ProducedQty > prevProductListing.ProducedQty)
                {
                    quantityComparisonType = QuantityComparisonType.Decreased;
                }
            }

            ReportProductEntryData reportProductEntryData = new ReportProductEntryData(
                productListing.CardData,
                productListing.ProducedQty,
                quantityComparisonType,
                productListing.SellQty
            );
            reportProductEntryDatas.Add(reportProductEntryData);
        }

        int moneyBefore = prevProgression.OwnedMoney;
        int totalMoneyCollected = currentProgression.MoneyCollected;
        int totalMoneySpent = currentProgression.MoneySpent;
        int remainingMoney = currentProgression.OwnedMoney;

        string timeStamp = "";

        switch(reportType)
        {
            case ReportType.Daily:
                timeStamp = $"Week {gameTimeManager.CurrentWeek} Day {gameTimeManager.CurrentDay}";
                break;
            case ReportType.Weekly:
                timeStamp = $"Week {gameTimeManager.CurrentWeek}";
                break;
        }

        ReportData reportData = new ReportData(
            reportType,
            timeStamp,
            reportProductEntryDatas,
            moneyBefore,
            totalMoneyCollected,
            totalMoneySpent,
            remainingMoney
        );

        OnReportGenerated?.Invoke(reportData);
    }
}
