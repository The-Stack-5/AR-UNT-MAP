using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MapNavigator : MonoBehaviour
{
    public Transform YouAreHere;
    public float walkSpeed = 0.6f;

    private Vector3 startClick;
    private Vector3 endClick;

    private bool hasStart;
    private bool hasEnd;
    private bool waitingForEnd;

    private List<Waypoint> currentPath = new List<Waypoint>();
    private List<Vector3> pathPoints = new List<Vector3>();

    private LineRenderer line;
    private Coroutine walkRoutine;

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

    public void AddClickPosition(Vector3 pos)
    {
        if (!waitingForEnd)
        {
            startClick = pos;
            hasStart = true;
            waitingForEnd = true;
        }
        else
        {
            endClick = pos;
            hasEnd = true;
            waitingForEnd = false;
        }
    }

    [ContextMenu("Find Path")]
    public void ComputePath()
    {
        if (!hasStart || !hasEnd)
            return;

        currentPath.Clear();
        pathPoints.Clear();
        line.positionCount = 0;

        if (walkRoutine != null)
            StopCoroutine(walkRoutine);

        Waypoint startNode = GetClosestWaypoint(startClick);
        Waypoint endNode = GetClosestWaypoint(endClick);

        if (startNode == null || endNode == null)
            return;

        currentPath = BFS(startNode, endNode);
        BuildPathPoints();
        DrawPath();

        if (YouAreHere != null && pathPoints.Count > 0)
            walkRoutine = StartCoroutine(WalkPath());
    }

    private Waypoint GetClosestWaypoint(Vector3 pos)
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

            if (current == goal)
                break;

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

    private void BuildPathPoints()
    {
        pathPoints.Add(startClick);

        foreach (Waypoint wp in currentPath)
            pathPoints.Add(wp.transform.position);

        pathPoints.Add(endClick);
    }

    private void DrawPath()
    {
        if (pathPoints.Count == 0)
            return;

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

            while (t < 1f)
            {
                t += (walkSpeed / dist) * Time.deltaTime;
                YouAreHere.position = Vector3.Lerp(start, target, t);
                yield return null;
            }

            YouAreHere.position = target;
        }
    }
}
