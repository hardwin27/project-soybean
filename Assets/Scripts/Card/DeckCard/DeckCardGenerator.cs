using UnityEngine;

[RequireComponent (typeof(DeckCardController))]
public class DeckCardGenerator : MonoBehaviour
{
    private DeckCardController controller;

    private void Awake()
    {
        controller = GetComponent<DeckCardController>();
        controller.OnDeckCardGenerated += GenerateCardFromDeck;
    }

    private void GenerateCardFromDeck(CardData cardData)
    {
        Vector2 randomPos = RandomValue.RandomPosAround(transform.position, 1.5f);
        CardGeneratorManager.Instance.GenerateCard(cardData, randomPos);
    }
}
