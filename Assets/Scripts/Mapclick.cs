using UnityEngine;

public class MapClick : MonoBehaviour
{
    public Camera mapCamera;
    public GameObject YouAreHere;
    public Collider2D[] Boundries;

    void Update()
    {
        if (mapCamera == null || YouAreHere == null || Boundries == null || Boundries.Length == 0)
        {
            Debug.LogError("MapClick: Missing references");
            return;
        }

        if (!Input.GetMouseButtonDown(0))
            return;

        Vector3 clickPos = mapCamera.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0f;

        // Check if click is inside any boundary collider
        foreach (Collider2D wall in Boundries)
        {
            if (wall != null && wall.OverlapPoint(clickPos))
            {
                // Valid click → move marker + notify navigator
                YouAreHere.transform.position = clickPos;
                Debug.Log("MapClick: YouAreHere moved to " + clickPos);

                MapNavigator nav = GetComponent<MapNavigator>();
                if (nav != null)
                {
                    nav.AddClickPosition(clickPos);
                }

                return;   // Stop after first valid collider match
            }
        }

        // Click was outside all boundaries → ignore silently
    }
}