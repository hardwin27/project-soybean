using UnityEngine;
using SingletonSystem;
using System;

public class CardGeneratorManager : Singleton<CardGeneratorManager>
{
    [SerializeField] private GameObject BaseCardPrefab;
    [SerializeField] private GameObject ToolCardPrefab;

    public Action<CardController> OnCardGenerated;

    public void GenerateCard(CardData cardData, Vector3 pos)
    {
        /*Debug.Log($"GenerateCard");*/
        GameObject generatedCardObj = null;

        if (cardData.CardType == CardType.Tool)
        {
            generatedCardObj = Instantiate(ToolCardPrefab);
        }
        else
        {
            generatedCardObj= Instantiate(BaseCardPrefab);
        }

        if (generatedCardObj != null) 
        {
            generatedCardObj.transform.position = pos;

            if (generatedCardObj.TryGetComponent(out CardController cardController))
            {
                cardController.AssignCardData(cardData);
                OnCardGenerated?.Invoke(cardController);
            }
        }
    }
}
