using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUiController : MonoBehaviour
{
    [Header("Tab Collection")]
    [SerializeField] private GameObject questTab;
    [SerializeField] private Button questButton;

    [SerializeField] private GameObject recipeTab;
    [SerializeField] private Button recipeButton;

    [SerializeField] private RectTransform mainTabRectTransform;
    [SerializeField] private float openTabXPos;
    [SerializeField] private float closeTabXPos;

    [SerializeField] private Button closeSideTabButton;

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

        closeSideTabButton.onClick.AddListener(CloseMainTab );
    }

    private void Start()
    {
        questTab.transform.SetAsLastSibling();

        questButton.onClick.AddListener(() =>
        {
            CloseAllTab();
            OpenMainTab();
            questTab.SetActive(true);
            OnUiTriggered?.Invoke("quest-tab");
            AudioManager.Instance.PlaySFX("ui_tab_changed");
        });

        recipeButton.onClick.AddListener(() =>
        {
            CloseAllTab();
            OpenMainTab();
            recipeTab.SetActive(true);
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

    private void CloseAllTab()
    {
        questTab.SetActive(false);
        recipeTab.SetActive(false);
    }

    private void OpenMainTab()
    {
        mainTabRectTransform.anchoredPosition = new Vector2(
                openTabXPos, mainTabRectTransform.anchoredPosition.y
            );
    }

    private void CloseMainTab()
    {
        mainTabRectTransform.anchoredPosition = new Vector2(
                closeTabXPos, mainTabRectTransform.anchoredPosition.y
            );
    }
}
