using UnityEngine;
using TMPro;

public class RoomInfo3D : MonoBehaviour
{
    [Header("Room Identity")]
    public string roomName;
    public int floorNumber = 1;

    [Header("Default Room Info")]
    public string defaultRoomType;
    public string defaultSchedule;

    [Header("Student Custom Info")]
    [TextArea(2, 4)]
    public string studentClassInfo;

    [Header("Label Settings")]
    public TextMeshPro roomLabel;
    public float yOffset = 2f;

    [Header("Navigation")]
    public Waypoint roomWaypoint; // Assign nearest waypoint

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (roomLabel != null)
        {
            roomLabel.gameObject.SetActive(false);
            roomLabel.transform.localPosition = new Vector3(0, yOffset, 0);
        }
    }

    void Update()
    {
        if (roomLabel != null && roomLabel.gameObject.activeSelf && mainCamera != null)
        {
            roomLabel.transform.LookAt(
                roomLabel.transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up
            );
        }
    }

    public void ShowInfo()
    {
        if (roomLabel == null) return;

        string infoToDisplay = !string.IsNullOrEmpty(studentClassInfo)
            ? $"Room: {roomName}\nFloor: {floorNumber}\n\nYour Class:\n{studentClassInfo}"
            : $"Room: {roomName}\nFloor: {floorNumber}\nType: {defaultRoomType}\nSchedule: {defaultSchedule}";

        roomLabel.text = infoToDisplay;
        roomLabel.color = floorNumber == 1 ? Color.cyan : Color.green;
        roomLabel.gameObject.SetActive(true);
    }

    public void HideInfo()
    {
        if (roomLabel != null)
            roomLabel.gameObject.SetActive(false);
    }
}