using UnityEngine;

public class DecorationUi : MonoBehaviour
{
    private DecorationManager decorationManager;
    [SerializeField] private DecorationOptionUi decorationOptionUiTemplate;
    [SerializeField] private Transform decorationOptionParent;

    private void Awake()
    {
        decorationManager = DecorationManager.Instance;

        if (decorationManager != null )
        {
            decorationManager.OnDecorationListingInitiated += InitiateOptionUis;
        }

    }

    private void InitiateOptionUis()
    {
        foreach (var decorationListing in decorationManager.DecorationListingData)
        {
            GameObject newDecorOptionObj = Instantiate(decorationOptionUiTemplate.gameObject, decorationOptionParent);
            DecorationOptionUi newDecorOptionUi = newDecorOptionObj.GetComponent<DecorationOptionUi>();
            newDecorOptionUi.AssignDecorationListing(decorationListing);
        }
    }
}
