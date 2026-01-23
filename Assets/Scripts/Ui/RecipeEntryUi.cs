using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeEntryUi : MonoBehaviour
{
    [SerializeField] private RecipeTileUi recipeTargetTileUi;
    [SerializeField] private List<RecipeTileUi> reqTileUis= new List<RecipeTileUi>();

    public int ReqTileUiCount
    {
        get
        {
            return (reqTileUis == null) ? 0 : reqTileUis.Count;
        }
    }

    public void AssignRecipe(RecipeData recipeData)
    {
        if (recipeData.GeneratedCard != null)
        {
            recipeTargetTileUi.TIleIconImage.sprite = recipeData.GeneratedCard.CardSprite;
            recipeTargetTileUi.TileNameText.text = recipeData.GeneratedCard.CardName;
        }
        else if (recipeData.ToolChanges != null)
        {
            recipeTargetTileUi.TIleIconImage.sprite = recipeData.ToolChanges[0].ToolCard.CardSprite;
            recipeTargetTileUi.TileNameText.text = recipeData.ToolChanges[0].ToolCard.CardName;
        }
        else
        {
            return;
        }

        if (recipeData.RecipeTargetSprite != null)
        {
            recipeTargetTileUi.TIleIconImage.sprite = recipeData.RecipeTargetSprite;
        }

        for (int x = 0; x < recipeData.CardCombos.Count; x++)
        {
            if (x >= reqTileUis.Count )
            {
                break;
            }

            reqTileUis[x].TIleIconImage.sprite = (recipeData.CardCombos[x].RecipeCardSprite == null) ?
                recipeData.CardCombos[x].CardData.CardSprite :
                recipeData.CardCombos[x].RecipeCardSprite;

            reqTileUis[x].TileNameText.text = recipeData.CardCombos[x].CardData.CardName;
        }
    }
}
