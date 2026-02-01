using UnityEngine;
using SingletonSystem;
using System.Collections.Generic;
using ReadOnlyEditor;
using System;

public class DecorationManager : Singleton<DecorationManager>
{
    [SerializeField] private List<DecorationCardData> decorationCardDatas = new List<DecorationCardData>();
    [SerializeField, ReadOnly] private List<DecorationListingData> decorationListingDatas;
    [SerializeField, ReadOnly] private List<DecorationCardController> spawnedDecoration = new List<DecorationCardController>();

    private CardGeneratorManager cardGeneratorManager;

    public Action OnDecorationListingInitiated;

    public List<DecorationListingData> DecorationListingData { get => decorationListingDatas; }

    private void Awake()
    {
        cardGeneratorManager = CardGeneratorManager.Instance;
    }

    private void Start()
    {
        decorationListingDatas = new List<DecorationListingData>();

        foreach (DecorationCardData decorationCardData in decorationCardDatas) 
        {
            decorationListingDatas.Add(new DecorationListingData(decorationCardData));
        }
        OnDecorationListingInitiated?.Invoke();
    }

    public DecorationCardController GenerateDecorationCard(DecorationListingData decorationListingData, Vector3 pos)
    {
        DecorationCardController newDecorationCardController = cardGeneratorManager.GenerateCard(decorationListingData.DecorationCardData, pos) as DecorationCardController;

        decorationListingData.AddStock(-1);
        newDecorationCardController.OnCardDestroyed += () =>
        {
            decorationListingData.AddStock(1);
        };

        return newDecorationCardController;
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
