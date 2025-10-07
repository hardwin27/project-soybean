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

    private void Start()
    {
        changeButton.onClick.AddListener(ChangeLayout);
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
}
