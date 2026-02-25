using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeEntryUi : MonoBehaviour
{
    [SerializeField] private List<RecipeTileUi> targetTileUis = new List<RecipeTileUi>();
    [SerializeField] private List<RecipeTileUi> reqTileUis= new List<RecipeTileUi>();

    public int TargetTileUiCount
    {
        get
        {
            return (targetTileUis == null) ? 0 : targetTileUis.Count;
        }
    }

    public int ReqTileUiCount
    {
        get
        {
            return (reqTileUis == null) ? 0 : reqTileUis.Count;
        }
    }

    public void AssignRecipe(RecipeData recipeData)
    {
        if (targetTileUis.Count <= 0 ||  reqTileUis.Count <= 0) return;

        if (recipeData.GeneratedCards.Count > 0)
        {
            for (int x = 0; x < recipeData.GeneratedCards.Count; x++)
            {
                targetTileUis[x].TIleIconImage.sprite = 
                    (recipeData.GeneratedCards[x].GeneratedCardSprite == null) ?
                    recipeData.GeneratedCards[x].CardData.CardSprite :
                    recipeData.GeneratedCards[x].GeneratedCardSprite;
                targetTileUis[x].TileNameText.text = 
                    (string.IsNullOrEmpty(recipeData.GeneratedCards[x].GeneratedCardName)) ?
                    recipeData.GeneratedCards[x].CardData.CardName :
                    recipeData.GeneratedCards[x].GeneratedCardName;
            }
        }
        else if (recipeData.ToolChanges != null)
        {
            targetTileUis[0].TIleIconImage.sprite = (recipeData.RecipeTargetSprite == null)?
                recipeData.ToolChanges[0].ToolCard.CardSprite :
                recipeData.RecipeTargetSprite;
            targetTileUis[0].TileNameText.text = string.IsNullOrEmpty(recipeData.RecipeSpecialTargetName) ? recipeData.ToolChanges[0].ToolCard.CardName : recipeData.RecipeSpecialTargetName;
        }
        else
        {
            return;
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

            reqTileUis[x].TileNameText.text = (string.IsNullOrEmpty(recipeData.CardCombos[x].RecipeCardName)) ?
                recipeData.CardCombos[x].CardData.CardName :
                recipeData.CardCombos[x].RecipeCardName;
        }
    }
}
