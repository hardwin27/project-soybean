using UnityEngine;

[CreateAssetMenu(fileName = "RecipeQuestData", menuName = "Quest/Recipe Quest Data")]
public class RecipeQuestData : QuestData
{
    [SerializeField] protected RecipeData targetRecipe;

    public RecipeData TargetRecipe { get => targetRecipe; }
}
