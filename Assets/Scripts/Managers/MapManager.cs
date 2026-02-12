using UnityEngine;
using UnityEngine.UI;

// Connects GPSHandler with UI map image and marker
public class MapManager : MonoBehaviour
{
    public Image mapImage;
    public RectTransform marker;
    public GeoBounds geoBounds;

    void Update()
    {
        if (GPSHandler.Instance == null || !GPSHandler.Instance.IsReady) return;

        float lat = GPSHandler.Instance.Latitude;
        float lon = GPSHandler.Instance.Longitude;

        Vector2 normalized = NormalizeGPS(lat, lon);
        Vector2 anchoredPos = new Vector2(
            (normalized.x - 0.5f) * mapImage.rectTransform.rect.width,
            (normalized.y - 0.5f) * mapImage.rectTransform.rect.height
        );

        marker.anchoredPosition = anchoredPos;
    }

    Vector2 NormalizeGPS(float lat, float lon)
    {
        float latRange = geoBounds.topLeftLatitude - geoBounds.bottomRightLatitude;
        float lonRange = geoBounds.bottomRightLongitude - geoBounds.topLeftLongitude;

        float normX = (lon - geoBounds.topLeftLongitude) / lonRange;
        float normY = (geoBounds.topLeftLatitude - lat) / latRange;
        return new Vector2(normX, normY);
    }
}
