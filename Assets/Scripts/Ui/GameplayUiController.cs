using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUiController : MonoBehaviour
{
    [Header("Tab Collection")]
    [SerializeField] private GameObject questTab;
    [SerializeField] private Button questButton;

    [SerializeField] private GameObject recipeTab;
    [SerializeField] private Button recipeButton;

    [SerializeField] private Button closeSideTabButotn;

    [SerializeField] private GameObject dayEndPanel;

    private GameTimeManager gameTimeManager;

    public Action<string> OnUiTriggered;

    private void Awake()
    {
        gameTimeManager = GameTimeManager.Instance;

        if (gameTimeManager != null )
        {
            gameTimeManager.OnDayStarted+= HandleOnDayStarted;
            gameTimeManager.OnDayEnded += HandleOnDayEnded;
        }
    }

    private void Start()
    {
        questTab.transform.SetAsLastSibling();

        questButton.onClick.AddListener(() =>
        {
            questTab.transform.SetAsLastSibling();
            OnUiTriggered?.Invoke("quest-tab");
            AudioManager.Instance.PlaySFX("ui_tab_changed");
        });

        recipeButton.onClick.AddListener(() =>
        {
            recipeTab.transform.SetAsLastSibling();
            OnUiTriggered?.Invoke("recipe-tab");
            AudioManager.Instance.PlaySFX("ui_tab_changed");
        });
    }

    private void HandleOnDayStarted()
    {
        dayEndPanel.SetActive(false);
    }

    private void HandleOnDayEnded()
    { 
        dayEndPanel.SetActive(true);
    }
}
