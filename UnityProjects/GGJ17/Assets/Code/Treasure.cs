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
    public Boid closestBoid;

    private void Start()
    {
        TreasureVisual.SetActive(false);
        boidFinder = GetComponentInChildren<BoidFinder>();
        if(!boidFinder)
        {
            Debug.Log("Uh oh. Treasure not set.");
        }
    }

    private void Update()
    {
        if(!closestBoid)
        {
            closestBoid = boidFinder.GetClosestBoid();
        }
        else
        {
            Debug.Log("Still no boid.");
        }
    }

    public void SetTreasureFound ()
    {
        Debug.Log("Treasure get!");
        IsFound = true;
        TreasureVisual.SetActive(true);
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
