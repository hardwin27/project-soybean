using System.Collections.Generic;
using UnityEngine;

public class DecorationDebugger : MonoBehaviour
{
    private DecorationManager decorationManager;

    [SerializeField] private List<DecorationCardData> decorationCards = new List<DecorationCardData>();
    [SerializeField] private int decorationAmount;

    private void Awake()
    {
        decorationManager = DecorationManager.Instance;
        decorationManager.OnDecorationListingInitiated += () =>
        {
            foreach (var card in decorationCards)
            {
                decorationManager.GainDecoration(card, decorationAmount);
            }
        };
    }
}
