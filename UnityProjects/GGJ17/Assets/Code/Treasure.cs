﻿using System.Collections;
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

    public BoidFinder BoidFinder;
    public Boid MyBoid;

    private void Start()
    {
        Hide();
        BoidFinder = GetComponentInChildren<BoidFinder>();
    }

    public void Hide()
    {
        OnHide.Invoke();
    }

    public void BoopBoid()
    {
        MyBoid.TestBoop();
    }

    void FixedUpdate()
    {
        if (!MyBoid)
            RegisterWithBoid();
    }

    void RegisterWithBoid()
    {
        if (!BoidFinder)
        {
            Debug.Log("Uh oh. No Boid Finder Configured on Treasure.");
        }
        else
        {
            MyBoid = BoidFinder.GetClosestBoid();
            if (!MyBoid)
                Debug.LogWarning("WARNING: No Closest Boid Found on Treasure");
            else
                MyBoid.IsTreasure = true;
        }
    }

    public void SetTreasureFound ()
    {
        if(!IsFound)
        {
            Debug.Log("Treasure get!");
            IsFound = true;
            OnFound.Invoke();
            MyBoid.IsTreasure = false;
        }
    }

    public void SetTreasureCollected ()
    {
        MetaScreen.current.CollectATreasure();
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
