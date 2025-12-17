using UnityEngine;
using UnityEngine.UI;

public class AutomationMenuUi : MonoBehaviour
{
    [SerializeField] private Transform automationOptionPanel;
    [SerializeField] private Transform recipeEntryParent;
    [SerializeField] private GameObject recipeEntryPrefab;
    [SerializeField] private GameObject cardIconUiPrefab;
    [SerializeField] private GameObject equalSignPrefab;
    [SerializeField] private GameObject plusSignPrefab;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (AutomationManager.Instance != null)
        {
            AutomationManager.Instance.OnAutomationCardSelected += OpenAutomationUi;
        }

        closeButton.onClick.AddListener(CloseAutomationUi);
    }

    private void OnDestroy()
    {
        if (AutomationManager.Instance != null)
        {
            AutomationManager.Instance.OnAutomationCardSelected -= OpenAutomationUi;
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(CloseAutomationUi);
        }
    }

    private void Start()
    {
        automationOptionPanel.gameObject.SetActive(false);

        CardComboManager cardComboManager = CardComboManager.Instance;

        foreach (var recipe in cardComboManager.Recipes)
        {
            if (recipe.GeneratedCard != null)
            {
                GameObject newRecipeEntryObj = Instantiate(recipeEntryPrefab, recipeEntryParent);
                GameObject targetCardObj = Instantiate(cardIconUiPrefab, newRecipeEntryObj.transform);
                Image targetCardImage = targetCardObj.GetComponent<Image>();
                targetCardImage.sprite = recipe.GeneratedCard.CardSprite;

                SimpleTooltip targetCardToolTip = targetCardObj.GetComponent<SimpleTooltip>();
                targetCardToolTip.iconSprite = recipe.GeneratedCard.CardSprite;

                Instantiate(equalSignPrefab, newRecipeEntryObj.transform);

                foreach (var card in recipe.CardCombos)
                {
                    if (card is ToolCardData toolCard)
                    {
                        GameObject toolCardObj = Instantiate(cardIconUiPrefab, newRecipeEntryObj.transform);
                        Image toolCardImage = toolCardObj.GetComponent<Image>();
                        toolCardImage.sprite = (toolCard.WithResourceSprite == null) ? toolCard.CardSprite : toolCard.WithResourceSprite;
                        SimpleTooltip toolCardToolTop = toolCardObj.GetComponent<SimpleTooltip>();
                        toolCardToolTop.iconSprite = (toolCard.WithResourceSprite == null) ? toolCard.CardSprite : toolCard.WithResourceSprite;
                    }
                    else
                    {
                        GameObject cardObj = Instantiate(cardIconUiPrefab, newRecipeEntryObj.transform);
                        Image cardImage = cardObj.GetComponent<Image>();
                        cardImage.sprite = card.CardSprite;
                        SimpleTooltip cardToolTop = cardObj.GetComponent<SimpleTooltip>();
                        cardToolTop.iconSprite = card.CardSprite;
                    }

                    if (recipe.CardCombos.IndexOf(card) < recipe.CardCombos.Count - 1)
                    {
                        Instantiate(plusSignPrefab, newRecipeEntryObj.transform);
                    }
                }

                Button recipeButton = newRecipeEntryObj.GetComponentInChildren<Button>();
                if (recipeButton != null) 
                {
                    Debug.Log($"{gameObject.name} Automation button found");
                    recipeButton.onClick.AddListener(() =>
                    {
                        Debug.Log($"automation click {recipe.GeneratedCard.name}");
                        AutomationManager.Instance.AssignRecipe(recipe);
                        CloseAutomationUi();
                    });
                }
            }
        }
    }

    private void OpenAutomationUi()
    {
        Debug.Log("OpenAutomationUi");
        automationOptionPanel.gameObject.SetActive(true);
    }

    private void CloseAutomationUi()
    {
        automationOptionPanel.gameObject.SetActive(false);
        if (AutomationManager.Instance != null)
        {
            AutomationManager.Instance.ClearSelectedAutomationCard();
        }
    }
}
