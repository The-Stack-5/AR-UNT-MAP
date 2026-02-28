using UnityEngine;
public class MapClick : MonoBehaviour
{
    public Camera mapCamera;
    public GameObject YouAreHere;
    public Collider2D[] Boundries;
    private bool isFirstClick = true; // ← only this one line added
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
        foreach (Collider2D wall in Boundries)
        {
            if (wall != null && wall.OverlapPoint(clickPos))
            {
                YouAreHere.transform.position = clickPos;
                if (isFirstClick)
                {
                    Debug.Log("First position selected at " + clickPos);
                    isFirstClick = false;
                }
                else
                {
                    Debug.Log("Second position selected at " + clickPos);
                    // Optional: isFirstClick = true; ← uncomment if you want it to loop forever
                }
                MapNavigator nav = GetComponent<MapNavigator>();
                if (nav != null)
                {
                    nav.AddClickPosition(clickPos);
                }
                return;
            }
        }
    }
}