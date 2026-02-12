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
                // Move the marker
                YouAreHere.transform.position = clickPos;
                Debug.Log("MapClick: YouAreHere moved to " + clickPos);

                // --- RoomInfo3D integration ---
                RoomInfo3D roomInfo = wall.GetComponent<RoomInfo3D>();
                if (roomInfo != null)
                {
                    // Show info for this room
                    roomInfo.ShowInfo();

                    // Hide info for all other rooms
                    foreach (var r in FindObjectsOfType<RoomInfo3D>())
                    {
                        if (r != roomInfo)
                            r.HideInfo();
                    }
                }

                // Notify navigator if you have one
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
