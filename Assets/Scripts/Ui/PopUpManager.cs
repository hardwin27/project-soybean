using UnityEngine;
using SingletonSystem;
using TMPro;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;
using System;

public class PopUpManager : Singleton<PopUpManager>
{
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;

    [SerializeField] private Button confirmButton;

    private Action onConfirmAction;

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = AudioManager.Instance;

        confirmButton.onClick.AddListener(ClosePopUp);
        popUpPanel.SetActive(false);
    }

    public void OpenPopUp(string title, string content, Action onConfirm = null)
    {
        titleText.text = title;
        contentText.text = content;
        onConfirmAction = onConfirm;
        popUpPanel.SetActive(true);
    }

    private void ClosePopUp()
    {
        audioManager.PlayDefaultUiClick();
        onConfirmAction?.Invoke();
        popUpPanel.SetActive(false);
    }
}
