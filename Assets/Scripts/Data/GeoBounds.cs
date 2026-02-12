using UnityEngine;

// Defines geographic bounds of map (top-left & bottom-right)
[CreateAssetMenu(fileName = "GeoBounds", menuName = "UNT AR MAP/Geo Bounds")]
public class GeoBounds : ScriptableObject
{
    [Header("Top Left Corner (lat, lon)")]
    public float topLeftLatitude;
    public float topLeftLongitude;

    [Header("Bottom Right Corner (lat, lon)")]
    public float bottomRightLatitude;
    public float bottomRightLongitude;
}
