using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SideTabData
{
    [SerializeField] private string tabName;
    [SerializeField] private GameObject tabPanel;
    [SerializeField] private int tabButton;
    [SerializeField] private TextMeshProUGUI tabNameText;
    [SerializeField] private string tabEventCode;

    public string TabName => tabName;
}

public class GameplayUiController : MonoBehaviour
{
    [SerializeField] private Sprite tabSelectedSprite;
    [SerializeField] private Sprite tabUnselectedSprite;
    [Header("Tab Collection")]
    [SerializeField] private List<SideTabData> tabs;
    [SerializeField] private GameObject questTab;
    [SerializeField] private Button questButton;

    [SerializeField] private GameObject recipeTab;
    [SerializeField] private Button recipeButton;

    [SerializeField] private GameObject officeTab;
    [SerializeField] private Button officeButton;

    [SerializeField] private GameObject employeeTab;
    [SerializeField] private Button employeeButton;

    [SerializeField] private GameObject eventTab;
    [SerializeField] private Button eventButton;

    [SerializeField] private RectTransform mainTabRectTransform;
    [SerializeField] private float openTabXPos;
    [SerializeField] private float closeTabXPos;

    [SerializeField] private Button closeSideTabButton;

    [SerializeField] private GameObject dayEndPanel;

    /*private GameTimeManager gameTimeManager;*/
    private GameManager gameManager;

    public Action<string> OnUiTriggered;

    private void Awake()
    {
        /*gameTimeManager = GameTimeManager.Instance;

        if (gameTimeManager != null )
        {
            gameTimeManager.OnDayStarted+= HandleOnDayStarted;
            gameTimeManager.OnDayEnded += HandleOnDayEnded;
        }*/

        foreach(var tab in tabs) 
        {
            
        }

        recipeTab.SetActive(true);
        questTab.SetActive(true);
        officeTab.SetActive(true);

        gameManager = GameManager.Instance;
        gameManager.OnDayStageStarted += HandleOnDayStageStarted;
        gameManager.OnDayStageEnded += HandleOnDayStageEnded;

        closeSideTabButton.onClick.AddListener(CloseMainTab);
    }

    private void Start()
    {
        CloseMainTab();

        questButton.onClick.AddListener(() =>
        {
            CloseAllTab();
            OpenMainTab();
            questTab.SetActive(true);
            questButton.image.sprite = tabSelectedSprite;
            OnUiTriggered?.Invoke("quest-tab");
            AudioManager.Instance.PlaySFXObject("ui_tab_changed");
        });

        recipeButton.onClick.AddListener(() =>
        {
            CloseAllTab();
            OpenMainTab();
            recipeTab.SetActive(true);
            recipeButton.image.sprite = tabSelectedSprite;
            OnUiTriggered?.Invoke("recipe-tab");
            AudioManager.Instance.PlaySFXObject("ui_tab_changed");
        });

        officeButton.onClick.AddListener(() =>
        {
           CloseAllTab();
           OpenMainTab();
           officeTab.SetActive(true);
           officeButton.image.sprite = tabSelectedSprite;
           OnUiTriggered?.Invoke("office-tab");
           AudioManager.Instance.PlaySFXObject("ui_tab_changed");
        });


    }

    private void HandleOnDayStageStarted()
    {
        dayEndPanel.SetActive(false);
    }

    private void HandleOnDayStageEnded()
    { 
        dayEndPanel.SetActive(true);
    }

    private void CloseAllTab()
    {
        questTab.SetActive(false);
        recipeTab.SetActive(false);
        officeTab.SetActive(false);

        questButton.image.sprite = tabUnselectedSprite;
        recipeButton.image.sprite = tabUnselectedSprite;
        officeButton.image.sprite = tabUnselectedSprite;
    }

    private void OpenMainTab()
    {
        mainTabRectTransform.anchoredPosition = new Vector2(
                openTabXPos, mainTabRectTransform.anchoredPosition.y
            );

        closeSideTabButton.gameObject.SetActive(true);
    }

    private void CloseMainTab()
    {
        mainTabRectTransform.anchoredPosition = new Vector2(
                closeTabXPos, mainTabRectTransform.anchoredPosition.y
            );

        closeSideTabButton.gameObject.SetActive(false);
        CloseAllTab();
    }
}
