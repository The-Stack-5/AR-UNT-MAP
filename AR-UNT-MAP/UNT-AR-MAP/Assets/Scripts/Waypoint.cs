using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> neighbors = new List<Waypoint>();

    public bool isConnector = false;           // True if this node connects floors
    public Waypoint connectedFloorNode = null; // Corresponding node on the other floor
    public int floor = 1;                       // 1 = Floor1, 2 = Floor2

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
            {
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }
    }
}