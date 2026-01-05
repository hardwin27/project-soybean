using System;
using UnityEngine;
using SingletonSystem;

public class AutomationManager : Singleton<AutomationManager>
{
    [SerializeField] private AutomationCardController selectedAutomationCard;

    public Action OnAutomationCardSelected;

    public void SelectAutomationCard(AutomationCardController automationCard)
    {
        selectedAutomationCard = automationCard;
        OnAutomationCardSelected?.Invoke();
    }

    public void ClearSelectedAutomationCard()
    {
        selectedAutomationCard = null;
    }

    public void AssignRecipe(RecipeData recipeData)
    {
        if (selectedAutomationCard != null) 
        {
            selectedAutomationCard.AssignRequiredCards(recipeData);
        }
    }
}
