using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float dragSpeed = 2f;
    [SerializeField] private float minX, maxX, minY, maxY;
    
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float smoothZoomTime = 0.1f;
    
    [Header("Layer Settings")]
    [SerializeField] private LayerMask blockingLayers = 1;
    
    [SerializeField, ReadOnly] private Vector3 dragOrigin;
    [SerializeField, ReadOnly] private bool isDragging = false;
    private Camera cam;
    private float targetZoom;
    private float zoomVelocity;

    private float MinX { get => minX; }
    private float MaxX { get => maxX; }
    private float MinY { get => minY; }
    private float MaxY { get => maxY; }


    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = Camera.main;
        }
        
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        HandleZoomInput();
        HandleDragInput();
    }

    void HandleDragInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsOverBlockingObject())
            {
                StartDrag();
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            ContinueDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    void HandleZoomInput()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0)
            return;

        Vector3 mouseWorldBefore = cam.ScreenToWorldPoint(Input.mousePosition);

        targetZoom -= scroll * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        float previousZoom = cam.orthographicSize;
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothZoomTime);

        Vector3 mouseWorldAfter = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 difference = mouseWorldBefore - mouseWorldAfter;
        transform.position += difference;

        ApplyZoomBounds();
    }


    bool IsOverBlockingObject()
    {
        /*Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, blockingLayers);
        
        return hit.collider != null;*/

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return true;

        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, blockingLayers);
        return hit.collider != null;
    }

    void StartDrag()
    {
        isDragging = true;
        dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        dragOrigin.z = transform.position.z;
    }

    void ContinueDrag()
    {
        Vector3 currentPos = cam.ScreenToWorldPoint(Input.mousePosition);
        currentPos.z = transform.position.z;

        Vector3 difference = dragOrigin - currentPos;

        Vector3 desiredPos = transform.position + difference * dragSpeed * Time.deltaTime;

        if (MinX != MaxX || MinY != MaxY)
        {
            desiredPos = ClampPositionToCameraBounds(desiredPos);
        }

        transform.position = desiredPos;
    }

    void EndDrag()
    {
        isDragging = false;
    }

    Vector3 ClampPositionToCameraBounds(Vector3 position)
    {
        // current camera half-extents (in world units)
        float vertExtentCurrent = cam.orthographicSize;
        float horizExtentCurrent = vertExtentCurrent * cam.aspect;

        // half-extents when at the most zoomed-out view (editor reference)
        float vertExtentMax = maxZoom;
        float horizExtentMax = vertExtentMax * cam.aspect;

        // how much extra room the camera center should gain when zoomed in
        // (this is zero when current == maxZoom; positive when zoomed in)
        float extraX = Mathf.Max(0f, horizExtentMax - horizExtentCurrent);
        float extraY = Mathf.Max(0f, vertExtentMax - vertExtentCurrent);

        // Editor min/max are center limits at maxZoom, so expand them by extra when zoomed in
        float minXBound = minX - extraX;
        float maxXBound = maxX + extraX;
        float minYBound = minY - extraY;
        float maxYBound = maxY + extraY;

        // Safeguard: if bounds inverted (user set too-small range for current view),
        // collapse to center of the editor-defined range to keep camera stable (no jitter)
        if (minXBound > maxXBound)
        {
            float centerX = (minX + maxX) * 0.5f;
            minXBound = maxXBound = centerX;
        }
        if (minYBound > maxYBound)
        {
            float centerY = (minY + maxY) * 0.5f;
            minYBound = maxYBound = centerY;
        }

        position.x = Mathf.Clamp(position.x, minXBound, maxXBound);
        position.y = Mathf.Clamp(position.y, minYBound, maxYBound);

        return position;
    }

    void ApplyZoomBounds()
    {
        if (MinX != MaxX || MinY != MaxY)
        {
            transform.position = ClampPositionToCameraBounds(transform.position);
        }
    }



    void OnDrawGizmosSelected()
    {
        if (MinX != MaxX || MinY != MaxY)
        {
            Gizmos.color = Color.green;
            Vector3 center = new Vector3((MinX + MaxX) * 0.5f, (MinY + MaxY) * 0.5f, transform.position.z);
            Vector3 size = new Vector3(MaxX - MinX, MaxY - MinY, 0.1f);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
