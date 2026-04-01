using UnityEngine;

public class ClassroomHotspot : MonoBehaviour
{
    [Header("Display")]
    public string classroomName;

    [Header("Path Destination")]
    public Transform destinationPoint; // doorway / hallway point to route to

    public Vector3 GetDestination()
    {
        if (destinationPoint != null)
            return destinationPoint.position;

        return transform.position;
    }
}