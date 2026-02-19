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
    [SerializeField] private Button tabButton;
    [SerializeField] private TextMeshProUGUI tabNameText;
    [SerializeField] private string tabEventCode;

    public string TabName => tabName;
    public GameObject TabPanel => tabPanel;
    public Button TabButton => tabButton;
    public TextMeshProUGUI TabNameText => tabNameText;
    public string TabEventCode => tabEventCode;
}

public class GameplayUiController : MonoBehaviour
{
    [SerializeField] private Sprite tabSelectedSprite;
    [SerializeField] private Sprite tabUnselectedSprite;
    [Header("Tab Collection")]
    [SerializeField] private List<SideTabData> tabs;

    [SerializeField] private RectTransform mainTabRectTransform;
    [SerializeField] private float openTabXPos;
    [SerializeField] private float closeTabXPos;

    [SerializeField] private Button closeSideTabButton;

    [SerializeField] private GameObject dayEndPanel;

    private GameManager gameManager;

    public Action<string> OnUiTriggered;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        gameManager.OnDayStageStarted += HandleOnDayStageStarted;
        gameManager.OnDayStageEnded += HandleOnDayStageEnded;

        closeSideTabButton.onClick.AddListener(CloseMainTab);

        foreach (var tab in tabs)
        {
            tab.TabNameText.text = tab.TabName;
            tab.TabPanel.SetActive(true);

            tab.TabButton.onClick.AddListener(() =>
            {
                CloseAllTab();
                OpenMainTab();
                tab.TabPanel.SetActive(true);
                tab.TabButton.image.sprite = tabSelectedSprite;
                OnUiTriggered?.Invoke(tab.TabEventCode);
                AudioManager.Instance.PlaySFXObject("ui_tab_button");
            });
        }
    }

    private void Start()
    {
        CloseMainTab();
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
        foreach(var tab in tabs)
        {
            tab.TabPanel.SetActive(false);
            tab.TabButton.image.sprite = tabUnselectedSprite;
        }
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

        AudioManager.Instance.PlaySFXObject("ui_on_side_panel_closed");
    }
}
