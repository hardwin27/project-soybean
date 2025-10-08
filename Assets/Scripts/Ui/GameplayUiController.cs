using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUiController : MonoBehaviour
{
    [SerializeField] private GameObject questTab;
    [SerializeField] private Button questButton;

    [SerializeField] private GameObject recipeTab;
    [SerializeField] private Button recipeButton;

    public Action<string> OnUiTriggered;

    private void Start()
    {
        questTab.transform.SetAsLastSibling();

        questButton.onClick.AddListener(() =>
        {
            questTab.transform.SetAsLastSibling();
            OnUiTriggered?.Invoke("quest-tab");
        });

        recipeButton.onClick.AddListener(() =>
        {
            recipeTab.transform.SetAsLastSibling();
            OnUiTriggered?.Invoke("recipe-tab");
        });
    }
}
