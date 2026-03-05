using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FitCameraToSprite : MonoBehaviour
{
    public SpriteRenderer target;
    public float padding = 0.5f;

    void Start()
    {
        if (!target) return;

        Camera cam = GetComponent<Camera>();
        cam.orthographic = true;

        Bounds b = target.bounds;

        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = b.size.x / b.size.y;

        float size = b.size.y / 2f;

        if (screenRatio >= targetRatio)
            cam.orthographicSize = size + padding;
        else
            cam.orthographicSize = (b.size.x / 2f) / screenRatio + padding;

        transform.position = new Vector3(b.center.x, b.center.y, transform.position.z);
    }
}