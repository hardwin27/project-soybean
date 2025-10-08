using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUi : MonoBehaviour
{
    [SerializeField] private SpriteRenderer layoutRenderer;
    [SerializeField] private List<Sprite> layoutSprites = new List<Sprite>();
    private int currentLayoutInd = 0;

    [SerializeField] private Button changeButton;
    [SerializeField] private GameObject debugUi;
    [SerializeField] private KeyCode hideDebugKeyCode;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        changeButton.onClick.AddListener(ChangeLayout);
        closeButton.onClick.AddListener(CloseGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(hideDebugKeyCode)) 
        {
            debugUi.SetActive(!debugUi.gameObject.activeInHierarchy);
        }
    }

    private void ChangeLayout()
    {
        currentLayoutInd++;
        if (currentLayoutInd >= layoutSprites.Count) 
        {
            currentLayoutInd = 0;
        }

        layoutRenderer.sprite = layoutSprites[currentLayoutInd];
    }

    private void CloseGame()
    {
        RuntimePlatform platform = Application.platform;

        // Check if the game is running on a PC (Windows or macOS)
        if (platform == RuntimePlatform.WindowsPlayer ||
            platform == RuntimePlatform.WindowsEditor ||
            platform == RuntimePlatform.OSXPlayer ||
            platform == RuntimePlatform.OSXEditor)
        {
            Application.Quit();
        }
    }
}
