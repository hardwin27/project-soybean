using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardProcessor : MonoBehaviour
{
    [SerializeField, ReadOnly] private bool isProcessing = false;
    [SerializeField] private Canvas cardCanvas;
    [SerializeField] private Slider progressSlider;

    [SerializeField, ReadOnly] private RecipeData processedRecipe;
    [SerializeField, ReadOnly] private List<CardController> processedStack;

    private float processDuration;
    private float processTimer;

    private void Awake()
    {
        ResetProcess();
    }

    private void Start()
    {
        cardCanvas.worldCamera = Camera.main;
        progressSlider.maxValue = 1f;
    }

    private void Update()
    {
        if (isProcessing) 
        { 
            if (processTimer < processDuration)
            {
                processTimer += Time.deltaTime;
                progressSlider.value = processTimer / processDuration;
            }
            else
            { 
                FinishProcess();
            }
        }
    }

    private void ResetProcess()
    {
        isProcessing = false;
        processedRecipe = null;
        processedStack = null;

        processDuration = 0f;
        processTimer = 0f;

        progressSlider.gameObject.SetActive(false);
    }

    private void FinishProcess()
    {
        isProcessing = false;

        foreach (CardController card in processedStack)
        {
            card.IsOnProcess = false;
        }

        if (processedRecipe.GeneratedCard != null) 
        {
            Vector2 randomPos = RandomPosAround(transform.position, 2.5f);

            CardGeneratorManager.Instance.GenerateCard(
                processedRecipe.GeneratedCard, 
                new Vector3(randomPos.x, randomPos.y, transform.position.z)
            );
        }

        if (processedRecipe.DestroyedCards.Count > 0)
        {
            foreach (var card in processedRecipe.DestroyedCards)
            {
                CardController cardToDestroy = processedStack.Find(c => c.CardData == card);
                if (cardToDestroy != null)
                {
                    cardToDestroy.gameObject.SetActive(false);
                }
            }
        }

        if (processedRecipe.ToolChanges.Count > 0)
        {
            List<ToolCardController> toolCardControllers = new List<ToolCardController>();

            foreach (var cardController in processedStack)
            {
                if (cardController is ToolCardController toolCardController)
                {
                    toolCardControllers.Add(toolCardController);
                }
            }

            foreach (var toolchange in processedRecipe.ToolChanges)
            {
                ToolCardController selectedTool = toolCardControllers.Find(t => t.ToolCardData == toolchange.ToolCard);
                if (selectedTool != null) 
                {
                    foreach (var statReq in toolchange.ToolStats)
                    {
                        RuntimeStat runtimeStat = selectedTool.RuntimeStats.Find(s => s.Stat == statReq.StatData);
                        if (runtimeStat != null) 
                        {
                            runtimeStat.SetValue(runtimeStat.CurrentValue + statReq.StatValue);
                        }
                    }
                }
            }
        }

        ResetProcess();
    }

    public void ProcessRecipe(RecipeData recipe, List<CardController> cardStack)
    {
        if (isProcessing)
        {
            return;
        }

        processedRecipe = recipe;
        processedStack = cardStack;

        processTimer = 0f;
        processDuration = recipe.ProcessDuration;

        foreach (CardController card in processedStack) 
        {
            card.IsOnProcess = true;
        }

        progressSlider.gameObject.SetActive(true);
        isProcessing = true;
    }

    private static Vector2 RandomPosAround(Vector2 center, float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        return center + new Vector2(x, y);
    }
}
