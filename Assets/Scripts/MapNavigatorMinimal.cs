using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MapNavigator : MonoBehaviour
{
    [Header("Marker")]
    public Transform YouAreHere;
    public float walkSpeed = 0.6f;

    [Header("Floors (optional visuals)")]
    public int currentFloor = 1;
    public GameObject floor1Root;
    public GameObject floor2Root;

    private LineRenderer line;
    private Coroutine walkRoutine;

    private Dictionary<string, Waypoint> waypointById = new Dictionary<string, Waypoint>();
    private List<Waypoint> currentPath = new List<Waypoint>();
    private List<Vector3> pathPoints = new List<Vector3>();

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        line.useWorldSpace = true;
        line.positionCount = 0;
        line.sortingOrder = 100;
    }

    void Start()
    {
        CacheWaypoints();
        SetFloor(currentFloor);
    }

    // Call this if you add/remove waypoints at runtime (usually not needed)
    public void CacheWaypoints()
    {
        waypointById.Clear();

        foreach (Waypoint wp in FindObjectsOfType<Waypoint>())
        {
            if (wp == null) continue;
            if (string.IsNullOrWhiteSpace(wp.roomId)) continue;

            if (!waypointById.ContainsKey(wp.roomId))
                waypointById.Add(wp.roomId, wp);
            else
                Debug.LogWarning($"Duplicate roomId '{wp.roomId}' found on '{wp.name}'. Fix IDs to be unique.");
        }
    }

    // UI should call this (room list -> startId, endId)
    public void ComputePathByRoomIds(string startRoomId, string endRoomId)
    {
        if (!waypointById.TryGetValue(startRoomId, out Waypoint startNode))
        {
            Debug.LogWarning($"Start roomId not found: {startRoomId}");
            return;
        }

        if (!waypointById.TryGetValue(endRoomId, out Waypoint endNode))
        {
            Debug.LogWarning($"End roomId not found: {endRoomId}");
            return;
        }

        // Clear previous visuals/movement
        currentPath.Clear();
        pathPoints.Clear();
        line.positionCount = 0;

        if (walkRoutine != null)
            StopCoroutine(walkRoutine);

        currentPath = BFS(startNode, endNode);

        if (currentPath == null || currentPath.Count == 0)
        {
            Debug.LogWarning("No path found between selected rooms. Check neighbor connections.");
            return;
        }

        BuildPathPointsFromWaypoints();
        DrawPath();

        // Optional: helpful debug directions
        Debug.Log(BuildDirectionsSummary(currentPath));

        // If you want floor to switch immediately to start floor:
        SetFloor(startNode.floorIndex);

        if (YouAreHere != null && pathPoints.Count > 0)
            walkRoutine = StartCoroutine(WalkPath());
    }

    private List<Waypoint> BFS(Waypoint start, Waypoint goal)
    {
        Queue<Waypoint> queue = new Queue<Waypoint>();
        Dictionary<Waypoint, Waypoint> cameFrom = new Dictionary<Waypoint, Waypoint>();
        HashSet<Waypoint> visited = new HashSet<Waypoint>();

        queue.Enqueue(start);
        visited.Add(start);
        cameFrom[start] = null;

        while (queue.Count > 0)
        {
            Waypoint current = queue.Dequeue();
            if (current == goal) break;

            foreach (Waypoint neighbor in current.neighbors)
            {
                if (neighbor == null || visited.Contains(neighbor))
                    continue;

                visited.Add(neighbor);
                cameFrom[neighbor] = current;
                queue.Enqueue(neighbor);
            }
        }

        if (!cameFrom.ContainsKey(goal))
            return new List<Waypoint>();

        List<Waypoint> path = new List<Waypoint>();
        Waypoint step = goal;

        while (step != null)
        {
            path.Insert(0, step);
            step = cameFrom[step];
        }

        return path;
    }

    private void BuildPathPointsFromWaypoints()
    {
        pathPoints.Clear();
        foreach (Waypoint wp in currentPath)
            pathPoints.Add(wp.transform.position);
    }

    private void DrawPath()
    {
        if (pathPoints.Count == 0) return;

        line.positionCount = pathPoints.Count;
        for (int i = 0; i < pathPoints.Count; i++)
            line.SetPosition(i, pathPoints[i]);
    }

    private IEnumerator WalkPath()
    {
        YouAreHere.position = pathPoints[0];

        for (int i = 1; i < pathPoints.Count; i++)
        {
            Vector3 start = YouAreHere.position;
            Vector3 target = pathPoints[i];

            float dist = Vector3.Distance(start, target);
            float t = 0f;

            // Avoid divide-by-zero if two points overlap
            float safeDist = Mathf.Max(dist, 0.0001f);

            while (t < 1f)
            {
                t += (walkSpeed / safeDist) * Time.deltaTime;
                YouAreHere.position = Vector3.Lerp(start, target, t);
                yield return null;
            }

            YouAreHere.position = target;

            // If we reached a waypoint, auto-switch floor when we step onto stairs
            Waypoint reached = FindClosestWaypointTo(target);
            if (reached != null && reached.isStairsConnector)
                SetFloor(reached.floorIndex);
        }
    }

    // Helper: closest waypoint (used only for "reached node" detection)
    private Waypoint FindClosestWaypointTo(Vector3 pos)
    {
        Waypoint closest = null;
        float best = float.MaxValue;

        foreach (Waypoint wp in FindObjectsOfType<Waypoint>())
        {
            float d = Vector3.Distance(pos, wp.transform.position);
            if (d < best)
            {
                best = d;
                closest = wp;
            }
        }
        return closest;
    }

    private void SetFloor(int floor)
    {
        currentFloor = floor;

        if (floor1Root != null) floor1Root.SetActive(currentFloor == 1);
        if (floor2Root != null) floor2Root.SetActive(currentFloor == 2);
    }

    private string BuildDirectionsSummary(List<Waypoint> path)
    {
        // Detect a floor change via stairs landing pair
        for (int i = 1; i < path.Count; i++)
        {
            var a = path[i - 1];
            var b = path[i];

            if (a.isStairsConnector && b.isStairsConnector && a.floorIndex != b.floorIndex)
            {
                return $"Route uses stairs: go from Floor {a.floorIndex} to Floor {b.floorIndex}.";
            }
        }

        // Or detect any floor change at all
        for (int i = 1; i < path.Count; i++)
        {
            if (path[i - 1].floorIndex != path[i].floorIndex)
                return $"Route changes floors (Floor {path[i - 1].floorIndex} -> Floor {path[i].floorIndex}).";
        }

        return "Route stays on the same floor.";
    }
}
