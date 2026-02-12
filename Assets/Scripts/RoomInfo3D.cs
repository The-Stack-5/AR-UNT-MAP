using UnityEngine;
using TMPro; // Optional: only if using TextMeshPro

public class RoomInfo3D : MonoBehaviour
{
    [Header("Room Details")]
    public string roomName;
    public string roomType;
    public string roomSchedule;

    [Header("Label Settings")]
    public TextMeshPro roomLabel; // Assign a TextMeshPro object above the room
    public float yOffset = 2f;    // How high above the room the label appears

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Hide label initially
        if (roomLabel != null)
            roomLabel.gameObject.SetActive(false);

        // Position the label above the room
        if (roomLabel != null)
            roomLabel.transform.localPosition = new Vector3(0, yOffset, 0);
    }

    void Update()
    {
        // Make label always face the camera (billboard effect)
        if (roomLabel != null && roomLabel.gameObject.activeSelf && mainCamera != null)
        {
            roomLabel.transform.LookAt(
                roomLabel.transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up
            );
        }
    }

    // Call this when the room is selected/clicked
    public void ShowInfo()
    {
        if (roomLabel != null)
        {
            roomLabel.text = $"{roomName}\n{roomType}\n{roomSchedule}";
            roomLabel.gameObject.SetActive(true);
        }
    }

    // Call this to hide the label
    public void HideInfo()
    {
        if (roomLabel != null)
            roomLabel.gameObject.SetActive(false);
    }
}
