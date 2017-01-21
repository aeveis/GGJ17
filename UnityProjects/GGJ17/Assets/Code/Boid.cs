using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boid : MonoBehaviour 
{
    //Inspector Variables
    public float GizmoRadius = 1f;
    public bool DisplayGizmosAlways = true;

    public UnityEvent OnPing;

    //Private Variables
    List<Boid> neighbors = new List<Boid>();

    public void Ping()
    {
        OnPing.Invoke();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Boop");

        Boid newNeighbor = col.GetComponent<Boid>();

        if (newNeighbor && neighbors.Contains(newNeighbor) == false)
            neighbors.Add(newNeighbor);
    }

    void OnDrawGizmos()
    {
        //if(DisplayGizmosAlways)
            //Gizmos.DrawSphere(transform.position, GizmoRadius / 2f);
    }

    void OnDrawGizmosSelected()
    {
        //Gizmos.DrawSphere(transform.position, GizmoRadius);
    }
}
