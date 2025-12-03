using System;
using UnityEngine;
using SingletonSystem;
using Sirenix.OdinInspector;

public class AutomationManager : Singleton<AutomationManager>
{
    [SerializeField, ReadOnly] private AutomationCardController selectedAutomationCard;

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
        
    }
}
