using UnityEngine;

public class MapClick : MonoBehaviour
{
    public Camera mapCamera;
    public GameObject YouAreHere;
    public Collider2D[] Boundries;

    void Update()
    {
        if (mapCamera == null || YouAreHere == null || Boundries == null || Boundries.Length == 0)
            return;

        if (!Input.GetMouseButtonDown(0))
            return;

        Vector3 clickPos = mapCamera.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0f;

        foreach (Collider2D wall in Boundries)
        {
            if (wall != null && wall.OverlapPoint(clickPos))
            {
                YouAreHere.transform.position = clickPos;
                return;
            }
        }
    }
}
