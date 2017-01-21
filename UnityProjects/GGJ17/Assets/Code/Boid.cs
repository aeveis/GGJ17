using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoopData
{
    public static int BoopCounter = 0;

    public int BoopID = -1;

    public float MaxValue = 1f;
    public float Rate = 1f;
    public float DecayRate = 1f;
    public Boid ParentBoid;

    float currentValue = 0f;
    bool decaying = false;

    public float CurrentValue { get { return currentValue; } }

    public BoopData(BoopData data)
    {
        BoopID = data.BoopID;
        MaxValue = data.MaxValue;
        Rate = data.Rate;
        DecayRate = data.DecayRate;
    }

    public BoopData(int id, float max, float rate, float decay)
    {
        BoopID = id;
        MaxValue = max;
        Rate = rate;
        DecayRate = decay;
    }

    //Evaluates this boid. Returns false if the Boid should be removed.
    public bool Evaluate()
    {
        if (!decaying)
        {
            if (currentValue < MaxValue)
                currentValue += Time.deltaTime * Rate;
            else
            {
                decaying = true;
                ParentBoid.SpreadInfection(this);
            }
        }
        else
        {
            if (currentValue > 0)
                currentValue -= Time.deltaTime * Rate;
            else
                return false;
        }
        return true;
    }
}

public class Boid : MonoBehaviour 
{
    //Inspector Variables
    [Header("Gizmos Parameters")]
    public float GizmoRadius = 1f;
    public Color GizmoColor = Color.red;

    public Transform BoidVisual;
    public float PingDecay = 0.1f;

    public UnityEvent OnInfected;

    //Private Variables
    List<Boid> neighbors = new List<Boid>();
    List<BoopData> activeBoops = new List<BoopData>();

    float totalValue = 0f;
    public float Value { get { return totalValue; } }

    void Update()
    {
        EvaluateBoops();

        if (BoidVisual)
            BoidVisual.transform.localScale = Vector3.one * (1 + totalValue);
    }

    void EvaluateBoops()
    {
        totalValue = 0f;

        for (int i = activeBoops.Count - 1; i >= 0; i--)
        {
            bool stillActive = activeBoops[i].Evaluate();
            totalValue += Mathf.Clamp(activeBoops[i].CurrentValue, 0f, Mathf.Infinity);

            if (!stillActive)
                activeBoops.RemoveAt(i);
        }
    }

    public void SpreadInfection(BoopData data)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            neighbors[i].Infect(data);
        }
    }

    public void Infect(BoopData data)
    {
        if (ContainsBoop(data.BoopID) == false)
        {
            BoopData newInfection = new BoopData(data);
            newInfection.MaxValue -= PingDecay;
            newInfection.ParentBoid = this;
            activeBoops.Add(newInfection);
            OnInfected.Invoke();
        }
    }

    public bool ContainsBoop(int boopID)
    {
        for (int i = 0; i < activeBoops.Count; i++)
        {
            if (activeBoops[i].BoopID == boopID)
                return true;
        }
        return false;
    }

    [ContextMenu("Create Boop")]
    public void TestBoop()
    {
        BoopData newInfection = new BoopData(BoopData.BoopCounter, 1f, 2f, 1.5f);
        newInfection.ParentBoid = this;
        BoopData.BoopCounter += 1;

        activeBoops.Add(newInfection);
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
