using UnityEngine;

public class MapClick : MonoBehaviour
{
    public Camera mapCamera;
    public GameObject YouAreHere;
    public Collider2D[] Boundries;
    public LayerMask classroomLayer;

    private bool isFirstClick = true;

    void Update()
    {
        if (mapCamera == null || YouAreHere == null || Boundries == null || Boundries.Length == 0)
        {
            Debug.LogError("MapClick: Missing references");
            return;
        }

        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 clickPos = mapCamera.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0f;

        // STEP 1: Check if user clicked a classroom hotspot
        Collider2D hit = Physics2D.OverlapPoint(clickPos, classroomLayer);
        if (hit != null)
        {
            ClassroomHotspot hotspot = hit.GetComponent<ClassroomHotspot>();
            if (hotspot != null)
            {
                Vector3 targetPos = hotspot.GetDestination();
                targetPos.z = 0f;

                Debug.Log("Clicked classroom: " + hotspot.classroomName + " -> routing to " + targetPos);

                HandleClickPosition(targetPos);
                return;
            }
        }

        // STEP 2: Otherwise, allow hallway clicks as before
        foreach (Collider2D wall in Boundries)
        {
            if (wall != null && wall.OverlapPoint(clickPos))
            {
                HandleClickPosition(clickPos);
                return;
            }
        }

        Debug.Log("Clicked outside valid area.");
    }

    private void HandleClickPosition(Vector3 pos)
    {
        YouAreHere.transform.position = pos;

        if (isFirstClick)
        {
            Debug.Log("First position selected at " + pos);
            isFirstClick = false;
        }
        else
        {
            Debug.Log("Second position selected at " + pos);
        }

        MapNavigator nav = GetComponent<MapNavigator>();
        if (nav != null)
        {
            nav.AddClickPosition(pos);
        }
    }
}