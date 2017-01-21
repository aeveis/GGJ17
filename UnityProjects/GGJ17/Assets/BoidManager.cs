using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityIntEvent : UnityEvent<int>
{   
}

public class BoidManager : MonoBehaviour 
{
    [HideInInspector]
    public static BoidManager current;

    public UnityIntEvent OnTreasurePinged;

    void Awake()
    {
        current = this;
    }

    public void TreasurePinged(int boopID)
    {
        Debug.Log("Treausre is Pinged!");
        OnTreasurePinged.Invoke(boopID);
    }
}
