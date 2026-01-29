using UnityEngine;

public class BuildingCardController : CardController
{
    protected void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Destroy(gameObject);
        }
    }

    
}

