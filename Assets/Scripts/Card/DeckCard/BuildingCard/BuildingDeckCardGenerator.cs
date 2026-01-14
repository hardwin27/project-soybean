using UnityEngine;

[RequireComponent (typeof(BuildingDeckCardController))]
public class BuildingDeckCardGenerator : MonoBehaviour
{
    private BuildingDeckCardController controller;

    private void Awake()
    {
        controller = GetComponent<BuildingDeckCardController>();
        controller.OnDeckCardGenerated += GenerateCardFromDeck;
    }

    private void GenerateCardFromDeck(CardData cardData)
    {
        Vector2 randomPos = RandomValue.RandomPosAround(transform.position, 2f);
        CardGeneratorManager.Instance.GenerateCard(cardData, randomPos);
    }
}
