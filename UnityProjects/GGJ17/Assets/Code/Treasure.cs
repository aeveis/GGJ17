using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Treasure : MonoBehaviour
{
    [Header("Gizmos Parameters")]
    public float GizmoRadius = 1f;
    public Color GizmoColor = Color.red;
    public bool GizmosOn = true;

    [Header("Visual Controls")]
    public bool IsFound = false;

    public UnityEvent OnHide;
    public UnityEvent OnFound;
    public UnityEvent OnCollected;

    public BoidFinder boidFinder;
    public Boid myBoid;

    private void Start()
    {
        Hide();
        boidFinder = GetComponentInChildren<BoidFinder>();
    }

    public void Hide()
    {
        OnHide.Invoke();
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
        if(!IsFound)
        {
            Debug.Log("Treasure get!");
            IsFound = true;
            OnFound.Invoke();
            myBoid.IsTreasure = false;
        }
    }

    public void SetTreasureCollected ()
    {
        OnCollected.Invoke();
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
