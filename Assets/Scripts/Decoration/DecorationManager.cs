using UnityEngine;
using SingletonSystem;
using System.Collections.Generic;
using ReadOnlyEditor;
using System;

public class DecorationManager : Singleton<DecorationManager>
{
    [SerializeField] private List<DecorationCardData> decorationCardDatas = new List<DecorationCardData>();
    [SerializeField, ReadOnly] private List<DecorationListingData> decorationListingDatas;

    public Action OnDecorationListingInitiated;

    public List<DecorationListingData> DecorationListingData { get => decorationListingDatas; }

    private void Start()
    {
        decorationListingDatas = new List<DecorationListingData>();

        foreach (DecorationCardData decorationCardData in decorationCardDatas) 
        {
            decorationListingDatas.Add(new DecorationListingData(decorationCardData));
        }
        OnDecorationListingInitiated?.Invoke();
    }

    public void GainDecoration(DecorationCardData gainedDecoration, int gainedAmount)
    {
        DecorationListingData selectedDecorationListing = decorationListingDatas.Find(d => d.DecorationCardData == gainedDecoration);
        if (selectedDecorationListing != null) 
        {
            selectedDecorationListing.AddStock(gainedAmount);
        }
    }
}
