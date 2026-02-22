using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlyEditor;
using System;

[RequireComponent(typeof(CardController))]
[RequireComponent(typeof(CardVisual))]
public class CardProcessor : MonoBehaviour
{
    [SerializeField] private bool isProcessing = false;
    [SerializeField] private Canvas cardCanvas;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private bool hideStackWhenProgressing = false;
    [SerializeField] private Color cardTintWhenProgression;
    [SerializeField] private Transform fixedSpawnPoint;

    [SerializeField, ReadOnly] private RecipeData processedRecipe;
    [SerializeField, ReadOnly] private List<CardController> processedStack;

    private CardController cardController;
    private CardVisual cardVisual;

    private float processDuration;
    private float processTimer;

    public Action OnProcessFinished;
    public Action OnProcessStarted;

    private void Awake()
    {
        cardController = GetComponent<CardController>();
        cardVisual = GetComponent<CardVisual>();
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

        /*AudioManager.Instance.PlaySFXObject("tile_on_combo_generate_tile");*/

        if (hideStackWhenProgressing)
        {
            cardVisual.TintVisual(Color.white);
        }

        foreach (CardController card in processedStack)
        {
            card.IsOnProcess = false;
            if (hideStackWhenProgressing)
            {
                if (card != cardController)
                {
                    if (card.TryGetComponent(out CardVisual visual))
                    {
                        visual.ToggleVisibility(true);
                    }
                }
            }
        }

        if (processedRecipe.GeneratedCards.Count > 0) 
        {
            foreach (var generatedCardData in  processedRecipe.GeneratedCards) 
            {
                int genCount = generatedCardData.GeneratedQty;
                while(genCount-- > 0) 
                {
                    Vector3 spawnPos;
                    if (fixedSpawnPoint == null)
                    {
                        Vector2 randomPos = RandomValue.RandomPosAround(transform.position, 1.5f);
                        spawnPos = new Vector3(randomPos.x, randomPos.y, transform.position.z);
                    }
                    else
                    {
                        spawnPos = fixedSpawnPoint.position;
                    }

                    CardGeneratorManager.Instance.GenerateCard(
                       generatedCardData.CardData,
                       spawnPos
                   );
                }
            }

            /*for (int i = 0; i < processedRecipe.GeneratedCardAmount; i++)
            {
                Vector2 randomPos = RandomValue.RandomPosAround(transform.position, 1.5f);

                CardGeneratorManager.Instance.GenerateCard(
                   processedRecipe.GeneratedCard,
                   new Vector3(randomPos.x, randomPos.y, transform.position.z)
               );


            }*/
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

        OnProcessFinished?.Invoke();

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
            if (hideStackWhenProgressing)
            {
                if (card != cardController)
                {
                    if (card.TryGetComponent(out CardVisual visual))
                    {
                        visual.ToggleVisibility(false);
                    }
                }
            }
            
        }

        if (hideStackWhenProgressing)
        {
            cardVisual.TintVisual(cardTintWhenProgression);
        }

        progressSlider.gameObject.SetActive(true);
        isProcessing = true;
        /*AudioManager.Instance.PlaySFXObject("process_started");*/

        OnProcessStarted?.Invoke();
    }
}
