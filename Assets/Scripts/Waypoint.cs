using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> neighbors = new List<Waypoint>();

<<<<<<< HEAD
=======
   // public bool isConnector = false;          
    //public Waypoint connectedFloorNode = null; 
   // public int floor = 1;                       

>>>>>>> origin/Testing-DEE
    public float gizmoRadius = 0.18f;
    public Color normalColor = Color.cyan;
    public Color selectedColor = Color.yellow;
    public Color connectionColor = Color.blue;

<<<<<<< HEAD
    

    void OnDrawGizmos()
    {
     
=======
    void OnDrawGizmos()
    {
>>>>>>> origin/Testing-DEE
        Gizmos.color = normalColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }

    void OnDrawGizmosSelected()
    {
<<<<<<< HEAD
       
=======
>>>>>>> origin/Testing-DEE
        Gizmos.color = selectedColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius * 1.3f);

        Gizmos.color = connectionColor;
        foreach (Waypoint neighbor in neighbors)
        {
            if (neighbor != null)
            {
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }
    }

<<<<<<< HEAD
  
}
=======
}

>>>>>>> origin/Testing-DEE
