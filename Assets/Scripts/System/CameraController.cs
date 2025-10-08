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
        HandleDragInput();
        HandleZoomInput();
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
        
        if (scroll != 0)
        {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
        
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothZoomTime);
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
        
        transform.position += difference * dragSpeed * Time.deltaTime;
        
        if (minX != maxX || minY != maxY)
        {
            ApplyCameraBounds();
        }
    }

    void EndDrag()
    {
        isDragging = false;
    }

    void ApplyCameraBounds()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;
    }

    void OnDrawGizmosSelected()
    {
        if (minX != maxX || minY != maxY)
        {
            Gizmos.color = Color.green;
            Vector3 center = new Vector3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, transform.position.z);
            Vector3 size = new Vector3(maxX - minX, maxY - minY, 0.1f);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
