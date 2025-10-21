using UnityEngine;

// Simple rotation/pulse for the "You are here" marker
public class YouAreHereMarker : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float pulseScale = 0.15f;
    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseScale;
        transform.localScale = baseScale * scale;
        transform.Rotate(Vector3.forward * 30 * Time.deltaTime);
    }
}
