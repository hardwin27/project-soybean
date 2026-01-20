using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ReadOnlyEditor;
using System.Collections.Generic;
using UnityEngine.InputSystem.iOS;

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
            GameObject newReportProductEntryUiObject = Object.Instantiate(reportProductEntryUiPrefab);
            newReportProductEntryUiObject.transform.parent = reportProductListingPanel.transform;
            if (newReportProductEntryUiObject.TryGetComponent(out ReportProductEntryUi reportProductEntryUi))
            {
                reportProductEntryUi.DisplayProductEntryData(reportProductEntryData);
                reportProductEntrieUis.Add(reportProductEntryUi);
            }
        }

        timeStamptText.text = reportData.TimeStamp;
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
    [SerializeField] private Sprite dailyConfiirmSprite;
    [SerializeField] private Sprite weekdlyConfirmSprite;

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
            reportConfrimImage.sprite = dailyConfiirmSprite;
        }
        else if (reportData.ReportType == ReportType.Weekly)
        {
            dailyReportUiData?.ToggleReport(false);
            weeklyReportUiData?.ToggleReport(true);
            usedReportUiData = weeklyReportUiData;
            reportConfrimImage.sprite = weekdlyConfirmSprite;
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
