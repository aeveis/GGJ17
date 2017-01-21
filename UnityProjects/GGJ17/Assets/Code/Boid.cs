using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoopData
{
    public static int BoopCounter = 0;

    public int BoopID = -1;
    public float value = 0f;
}

public class Boid : MonoBehaviour 
{
    //Inspector Variables
    [Header("Gizmos Parameters")]
    public float GizmoRadius = 1f;
    public Color GizmoColor = Color.red;

    public Transform BoidVisual;
    public float PingDecay = 0.001f;
    public float PingMagnifier = 2f;
    [Range(0.0001f, 10f)]
    public float PingFriction = 1f;

    public UnityEvent OnPing;

    //Private Variables
    List<Boid> neighbors = new List<Boid>();
    List<BoopData> activeBoops = new List<BoopData>();

    float currentValue = 0f;
    public float Value { get { return currentValue; } }

    void Update()
    {
        if (currentValue > PingDecay)
            ExertPressure();
        else
            currentValue = 0;

        if (BoidVisual)
            BoidVisual.transform.localScale = Vector3.one * (1 + currentValue);
    }

    void ExertPressure()
    {
        float pressureToRemove = 0f;
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i].Value < currentValue)
            {
                float pressureToNeighbor = currentValue * PingMagnifier * Time.deltaTime / PingFriction;
                pressureToRemove += pressureToNeighbor;
                neighbors[i].Ping(pressureToNeighbor);
            }
        }
        currentValue -= pressureToRemove;
    }

    [ContextMenu("Test Ping")]
    public void TestPing()
    {
        Ping(10f);
    }

    public void Ping(float pingStrength)
    {
        currentValue += pingStrength;
        OnPing.Invoke();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Boid newNeighbor = col.GetComponent<Boid>();

        if (newNeighbor && neighbors.Contains(newNeighbor) == false)
            neighbors.Add(newNeighbor);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = GizmoColor;

        for (int i = 0; i < neighbors.Count; i++)
        {
            Gizmos.DrawSphere(neighbors[i].transform.position, GizmoRadius);
        }
    }
}
