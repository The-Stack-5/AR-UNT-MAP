using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapCameraControls : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float panSpeed = 1f;

    // Set these to reasonable bounds for your map
    public float minZoom = 5f;
    public float maxZoom = 40f;

    private Camera cam;
    private Vector3 lastMouseWorld;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
    }

    void Update()
    {
        HandleZoom();
        HandlePan();
    }

    void HandleZoom()
    {
        // Mouse wheel zoom (WebGL friendly)
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    void HandlePan()
    {
        // Click + drag pan
        if (Input.GetMouseButtonDown(0))
            lastMouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            Vector3 currentMouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 delta = lastMouseWorld - currentMouseWorld;

            // Move camera opposite the drag
            transform.position += new Vector3(delta.x, delta.y, 0f) * panSpeed;

            lastMouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}