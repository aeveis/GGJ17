using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [Header("Gizmos Parameters")]
    public float GizmoRadius = 1f;
    public Color GizmoColor = Color.red;
    public bool GizmosOn = true;

    [Header("Visual Controls")]
    public GameObject TreasureVisual;
    public bool IsFound = false;

    public BoidFinder boidFinder;
    public Boid myBoid;

    private void Start()
    {
        TreasureVisual.SetActive(false);
        boidFinder = GetComponentInChildren<BoidFinder>();
    }

    void FixedUpdate()
    {
        if (!myBoid)
            RegisterWithBoid();
    }

    void RegisterWithBoid()
    {
        if (!boidFinder)
        {
            Debug.Log("Uh oh. No Boid Finder Configured on Treasure.");
        }
        else
        {
            myBoid = boidFinder.GetClosestBoid();
            if (!myBoid)
                Debug.LogWarning("WARNING: No Closest Boid Found on Treasure");
            else
                myBoid.IsTreasure = true;
        }
    }

    public void SetTreasureFound ()
    {
        Debug.Log("Treasure get!");
        IsFound = true;
        TreasureVisual.SetActive(true);
        myBoid.IsTreasure = false;
    }

    void OnDrawGizmosSelected()
    {
        if(GizmosOn)
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawSphere(transform.position, GizmoRadius);
        }     
    }

}
