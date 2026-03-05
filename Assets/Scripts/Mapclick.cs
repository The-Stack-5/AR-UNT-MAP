<<<<<<< HEAD
using UnityEngine;

=======
﻿using UnityEngine;
>>>>>>> origin/Testing-DEE
public class MapClick : MonoBehaviour
{
    public Camera mapCamera;
    public GameObject YouAreHere;
    public Collider2D[] Boundries;
<<<<<<< HEAD

=======
    private bool isFirstClick = true; 
>>>>>>> origin/Testing-DEE
    void Update()
    {
        if (mapCamera == null || YouAreHere == null || Boundries == null || Boundries.Length == 0)
        {
            Debug.LogError("MapClick: Missing references");
            return;
        }
<<<<<<< HEAD

        if (!Input.GetMouseButtonDown(0))
            return;

        Vector3 clickPos = mapCamera.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0f;

        // Check if click is inside any boundary collider
=======
        if (!Input.GetMouseButtonDown(0)) return;
        Vector3 clickPos = mapCamera.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0f;
>>>>>>> origin/Testing-DEE
        foreach (Collider2D wall in Boundries)
        {
            if (wall != null && wall.OverlapPoint(clickPos))
            {
<<<<<<< HEAD
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
=======
                YouAreHere.transform.position = clickPos;
                if (isFirstClick)
                {
                    Debug.Log("First position selected at " + clickPos);
                    isFirstClick = false;
                }
                else
                {
                    Debug.Log("Second position selected at " + clickPos);
                    
                }
>>>>>>> origin/Testing-DEE
                MapNavigator nav = GetComponent<MapNavigator>();
                if (nav != null)
                {
                    nav.AddClickPosition(clickPos);
                }
<<<<<<< HEAD

                return;   // Stop after first valid collider match
            }
        }

        // Click was outside all boundaries → ignore silently
    }
=======
                return;
            }
        }
    }

>>>>>>> origin/Testing-DEE
}
