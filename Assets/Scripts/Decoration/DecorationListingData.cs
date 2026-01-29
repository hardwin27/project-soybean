using UnityEngine;
using ReadOnlyEditor;
using System;

[System.Serializable]
public class DecorationListingData
{
    [SerializeField, ReadOnly] private DecorationCardData decorationCardData;
    [SerializeField, ReadOnly] private bool isUnlocked;
    [SerializeField, ReadOnly] private int stockAmount;

    public Action OnIsUnlockedUpdated;
    public Action OnStockUpdated;

    public DecorationCardData DecorationCardData { get => decorationCardData; }
    public bool IsUnlocked { get => isUnlocked; }
    public int StockAmount { get => stockAmount; }

    public DecorationListingData(DecorationCardData _decorationCardData)
    {
        decorationCardData = _decorationCardData;
        isUnlocked = false;
        stockAmount = 0;
    }
}
