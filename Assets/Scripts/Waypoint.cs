using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Identity")]
    public string roomId;          // e.g. "F1-110", "F2-250", "STAIRS_A_F1"
    public int floorIndex = 1;     // 1 or 2

    [Header("Connector Flags")]
    public bool isStairsConnector = false;

    [Header("Graph")]
    public List<Waypoint> neighbors = new List<Waypoint>();

    [Header("Gizmos")]
    public float gizmoRadius = 0.18f;
    public Color normalColor = Color.cyan;
    public Color selectedColor = Color.yellow;
    public Color connectionColor = Color.blue;

    void OnDrawGizmos()
    {
        Gizmos.color = normalColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = selectedColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius * 1.3f);

        Gizmos.color = connectionColor;
        foreach (Waypoint neighbor in neighbors)
        {
            if (neighbor != null)
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
}
