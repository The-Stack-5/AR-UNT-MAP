using UnityEngine;
using System.Collections;

// Handles GPS initialization, updates, and mock location for Unity Editor
public class GPSHandler : MonoBehaviour
{
    public static GPSHandler Instance { get; private set; }
    public float Latitude { get; private set; }
    public float Longitude { get; private set; }
    public bool IsReady { get; private set; }

    IEnumerator Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            yield break; // <-- was 'return;' (illegal in iterator)
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
        // Mock location for Editor play mode
        Latitude = 33.253f;     // Discovery Park mock
        Longitude = -97.152f;
        IsReady = true;
        yield break;
#endif

        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("GPS not enabled on device");
            yield break; // <-- iterator-friendly exit
        }

        // desiredAccuracyInMeters, updateDistanceInMeters
        Input.location.Start(1f, 0.1f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0 || Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogWarning("GPS timeout or failed");
            yield break;
        }

        IsReady = true;
    }

    void Update()
    {
        if (IsReady && Input.location.status == LocationServiceStatus.Running)
        {
            Latitude = Input.location.lastData.latitude;
            Longitude = Input.location.lastData.longitude;
        }
    }
}
