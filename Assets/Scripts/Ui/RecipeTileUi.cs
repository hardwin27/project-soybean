using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeTileUi : MonoBehaviour
{
    [SerializeField] private Image tileIconImage;
    [SerializeField] private TextMeshProUGUI tileNameText;

    public Image TIleIconImage { get { return tileIconImage; } }
    public TextMeshProUGUI TileNameText { get {  return tileNameText; } }
}
