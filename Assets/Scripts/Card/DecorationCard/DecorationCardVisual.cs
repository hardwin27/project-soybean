using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DecorationCardController))]
public class DecorationCardVisual : CardVisual
{
    private DecorationCardController decorationCardController;

    [SerializeField] private TextMeshProUGUI decorationNameHoverText;
    [SerializeField] private Button flipVisualButton;
    [SerializeField] private Button stashDecorationButton;

    protected override void Awake()
    {
        base.Awake();

        decorationCardController = cardController as DecorationCardController;

        flipVisualButton.onClick.AddListener(FlipVisual);
        stashDecorationButton.onClick.AddListener(() => decorationCardController.StashDecoration());
    }

    protected override void UpdateHoverDisplay()
    {
        return;
    }

    protected void FlipVisual()
    {
        cardRenderer.flipX = !cardRenderer.flipX;
    }
}
