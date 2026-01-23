using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUi : MonoBehaviour
{
    [SerializeField] private Transform recipeEntryParent;
    [SerializeField] private List<RecipeEntryUi> recipeEntryTemplates = new List<RecipeEntryUi>();

    /*[SerializeField] private GameObject recipeEntryPrefab;
    [SerializeField] private GameObject cardIconUiPrefab;
    [SerializeField] private GameObject equalSignPrefab;
    [SerializeField] private GameObject plusSignPrefab;*/

    private void Start()
    {
        CardComboManager cardComboManager = CardComboManager.Instance;

        foreach (var recipe in cardComboManager.Recipes) 
        {
            RecipeEntryUi recipeEntryTemplate = recipeEntryTemplates.Find(temp => temp.ReqTileUiCount == recipe.CardCombos.Count);
            if (recipeEntryTemplate != null ) 
            {
                GameObject newRecipeEntryObject = Instantiate(recipeEntryTemplate.gameObject, recipeEntryParent);
                RecipeEntryUi newRecipeEntryUi = newRecipeEntryObject.GetComponent<RecipeEntryUi>();

                newRecipeEntryUi.AssignRecipe(recipe);
            }
        }

        /*foreach (var recipe in cardComboManager.Recipes) 
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
                    if (card.CardData is ToolCardData toolCard)
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
                        cardImage.sprite = card.CardData.CardSprite;
                        SimpleTooltip cardToolTop = cardObj.GetComponent<SimpleTooltip>();
                        cardToolTop.iconSprite = card.CardData.CardSprite;
                    }

                    if (recipe.CardCombos.IndexOf(card) < recipe.CardCombos.Count - 1)
                    {
                        Instantiate(plusSignPrefab, newRecipeEntryObj.transform);
                    }
                }
            }
        }*/
    }
}
