using UnityEngine;

public class WaypointConnector : MonoBehaviour
{
    [Header("Connection Settings")]
    public float maxConnectionDistance = 5f;

    void Start()
    {
        Waypoint[] allWaypoints = FindObjectsOfType<Waypoint>();

        foreach (Waypoint wp in allWaypoints)
        {
            foreach (Waypoint other in allWaypoints)
            {
                if (wp == other) continue;

                if (!wp.neighbors.Contains(other) &&
                    Vector3.Distance(wp.transform.position, other.transform.position) <= maxConnectionDistance)
                {
                    wp.neighbors.Add(other);
                    other.neighbors.Add(wp);
                }
            }
        }

        Debug.Log("Waypoints connected automatically.");
    }
}