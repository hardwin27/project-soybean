using System.Collections;
using UnityEngine;

[RequireComponent (typeof(BuildingDeckCardController))]
public class BuildingDeckCardGenerator : MonoBehaviour
{
    private BuildingDeckCardController controller;

    [SerializeField] private Transform fixedSpawnPoint;

    private void Awake()
    {
        controller = GetComponent<BuildingDeckCardController>();
        controller.OnDeckCardGenerated += GenerateCardFromDeck;
    }

    private void GenerateCardFromDeck(CardData cardData)
    {
        StartCoroutine(GenerateCardCorountine(cardData));   
    }

    private IEnumerator GenerateCardCorountine(CardData cardData)
    {
        Vector3 spawnPos;

        if (fixedSpawnPoint != null)
        {
            spawnPos = fixedSpawnPoint.position;
        }
        else
        {
            Vector2 randomPos = RandomValue.RandomPosAround(transform.position, 2f);
            spawnPos = new Vector3(randomPos.x, randomPos.y, transform.position.z);
        }
        CardGeneratorManager.Instance.GenerateCard(cardData, spawnPos);
        yield return new WaitForEndOfFrame();
    }
}
