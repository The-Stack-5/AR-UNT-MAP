using UnityEngine;
using TMPro;

public class RoomInfo3D : MonoBehaviour
{
    [Header("Room Identity")]
    public string roomName;
    public int floorNumber; // 1 or 2

    [Header("Default Room Info")]
    public string defaultRoomType;
    public string defaultSchedule;

    [Header("Student Custom Info")]
    [TextArea(2,4)]
    public string studentClassInfo;   // Students can enter their own info here

    [Header("Label Settings")]
    public TextMeshPro roomLabel;
    public float yOffset = 2f;

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

        string infoToDisplay;

        if (!string.IsNullOrEmpty(studentClassInfo))
        {
            // If student entered their own class info
            infoToDisplay = $"Room: {roomName}\nFloor: {floorNumber}\n\nYour Class:\n{studentClassInfo}";
        }
        else
        {
            // Default room information
            infoToDisplay = $"Room: {roomName}\nFloor: {floorNumber}\nType: {defaultRoomType}\nSchedule: {defaultSchedule}";
        }

        roomLabel.text = infoToDisplay;

        // Color by floor
        if (floorNumber == 1)
            roomLabel.color = Color.cyan;      // Floor 1 = Blue
        else if (floorNumber == 2)
            roomLabel.color = Color.green;     // Floor 2 = Green

        roomLabel.gameObject.SetActive(true);
    }

    public void HideInfo()
    {
        if (roomLabel != null)
            roomLabel.gameObject.SetActive(false);
    }
}
