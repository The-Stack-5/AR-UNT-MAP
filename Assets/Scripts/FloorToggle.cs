using UnityEngine;

public class FloorToggle : MonoBehaviour
{
    // References to the floor GameObjects
    public GameObject Discovery_Floor1;
    public GameObject Discovery_Floor2;

    // Is Floor 1 currently active?
    private bool Discovery_Floor1_Active = true;



    // Allows calling from Inspector menu
    [ContextMenu("Toggle Floor")]
    public void ToggleFloor()
    {
        // Flip which floor is active
        Discovery_Floor1_Active = !Discovery_Floor1_Active;

        // Enable / disable floors
        Discovery_Floor1.SetActive(Discovery_Floor1_Active);
        Discovery_Floor2.SetActive(!Discovery_Floor1_Active);
    }
}
