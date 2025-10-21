using UnityEngine;
using System.Collections;

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
            yield break;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
        Latitude = 33.253f;   // mock point near Discovery Park
        Longitude = -97.152f;
        IsReady = true;
        while (true) { yield return null; } // keeps coroutine valid in Editor
#else
        if (!Input.location.isEnabledByUser) { Debug.LogWarning("GPS disabled"); yield break; }
        Input.location.Start(1f, 0.1f);
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait-- > 0)
            yield return new WaitForSeconds(1);
        if (maxWait <= 0 || Input.location.status != LocationServiceStatus.Running)
        { Debug.LogWarning("GPS timeout/failed"); yield break; }
        IsReady = true;
#endif
    }

#if !UNITY_EDITOR
    void Update()
    {
        if (IsReady && Input.location.status == LocationServiceStatus.Running)
        {
            Latitude = Input.location.lastData.latitude;
            Longitude = Input.location.lastData.longitude;
        }
    }
#endif
}
