using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ReadOnlyEditor;
using System.Collections.Generic;

[System.Serializable]
public class ReportUiData
{
    [SerializeField] private GameObject reportPanel;
    [SerializeField] private TextMeshProUGUI timeStamptText;
    [SerializeField] private GameObject reportProductListingPanel;
    [SerializeField, ReadOnly] private List<ReportProductEntryUi> reportProductEntrieUis = new List<ReportProductEntryUi>();
    [SerializeField] private TextMeshProUGUI moneyBeforeText;
    [SerializeField] private TextMeshProUGUI totalMoneyCollectedText;
    [SerializeField] private TextMeshProUGUI totalMoneySpentText;
    [SerializeField] private TextMeshProUGUI remainingMoney;

    public void ToggleReport(bool isEnabled)
    {
        reportPanel?.SetActive(isEnabled);
    }

    public void UpdateReport(ReportData reportData, GameObject reportProductEntryUiPrefab)
    {
        foreach (var reportProductUi in reportProductEntrieUis)
        {
            GameObject.Destroy(reportProductUi.gameObject);
        }

        reportProductEntrieUis.Clear();

        foreach( var reportProductEntryData in reportData.ReportProductEntryDatas)
        {
            GameObject newReportProductEntryUiObject = Object.Instantiate(reportProductEntryUiPrefab, reportProductListingPanel.transform);
            if (newReportProductEntryUiObject.TryGetComponent(out ReportProductEntryUi reportProductEntryUi))
            {
                reportProductEntryUi.DisplayProductEntryData(reportProductEntryData);
                reportProductEntrieUis.Add(reportProductEntryUi);
            }
        }

        switch (reportData.ReportType)
        {
            case ReportType.Daily:
                timeStamptText.text = $"Week {reportData.ReportWeekPeriod} Day {reportData.ReportDayPeriod}";
                break;
            case ReportType.Weekly:
                timeStamptText.text = $"Week {reportData.ReportWeekPeriod}";
                break;
        }

        moneyBeforeText.text = $"{reportData.MoneyBefore}";
        totalMoneyCollectedText.text = $"+{reportData.TotalMoneyCollected}";
        totalMoneySpentText.text = $"-{reportData.TotalMoneySpent}";
        remainingMoney.text = $"{reportData.RemainingMoney}";
    }
}

public class ReportSystemUi : MonoBehaviour
{
    [SerializeField] private ReportGenerator reportGenerator;
    [SerializeField] private GameObject reportProductEntryUiPrefab;
    [SerializeField] private ReportUiData dailyReportUiData;
    [SerializeField] private ReportUiData weeklyReportUiData;

    [SerializeField] private Button reportConfirmButton;
    [SerializeField] private Image reportConfrimImage;
    [SerializeField] private Sprite nextDayConfirmSprite;
    [SerializeField] private Sprite showWeeklyConfirmSprite;

    private void Awake()
    {
        reportGenerator.OnReportGenerated += HandleReportGenerated;
    }

    private void HandleReportGenerated(ReportData reportData)
    {
        ReportUiData usedReportUiData = null;

        if (reportData.ReportType == ReportType.Daily)
        {
            dailyReportUiData?.ToggleReport(true);
            weeklyReportUiData?.ToggleReport(false);
            usedReportUiData = dailyReportUiData;
            if (reportData.ReportDayPeriod < 7)
            {
                reportConfrimImage.sprite = nextDayConfirmSprite;
            }
            else
            {
                reportConfrimImage.sprite = showWeeklyConfirmSprite;
            }    
        }
        else if (reportData.ReportType == ReportType.Weekly)
        {
            dailyReportUiData?.ToggleReport(false);
            weeklyReportUiData?.ToggleReport(true);
            usedReportUiData = weeklyReportUiData;
            reportConfrimImage.sprite = nextDayConfirmSprite;
        }

        if (usedReportUiData != null)
        {
            usedReportUiData.UpdateReport(reportData, reportProductEntryUiPrefab);
            reportConfirmButton.onClick.RemoveAllListeners();
            reportConfirmButton.onClick.AddListener(() =>
            {
                reportGenerator.ConfirmReport(reportData.ReportType);
            });
        }
    }
}
