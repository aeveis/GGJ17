using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoopData
{
    [HideInInspector]
    public static int BoopCounter = 0;

    [HideInInspector]
    public int BoopID = -1;

    public float InfectionPercent = 0.2f;
    public float ValueMult = 1f;
    public float TimeMult = 1f;
    public float GenerationalDecay = 0.1f;
    public float AdditiveDecay = 0.001f;
    public AnimationCurve BoopCurve;

    [HideInInspector]
    public Boid ParentBoid;

    float currentEvaluation = 0f;
    float currentValue = 0f;
    bool decaying = false;
    bool pingedTreasure = false;

    public float CurrentValue { get { return currentValue; } }
    public float CurrentTime { get { return currentEvaluation; } }
    public bool IsTreasureBoop { get { return pingedTreasure; } set { pingedTreasure = value; } }

    public BoopData(BoopData data, bool generateNewID)
    {
        if (!generateNewID)
            BoopID = data.BoopID;
        else
        {
            BoopID = BoopCounter;
            BoopCounter++;
        }
        BoopCurve = data.BoopCurve;
        ValueMult = data.ValueMult;
        TimeMult = data.TimeMult;
        InfectionPercent = data.InfectionPercent;
        GenerationalDecay = data.GenerationalDecay;
        AdditiveDecay = data.AdditiveDecay;
        pingedTreasure = data.IsTreasureBoop;
    }

    //Evaluates this boid. Returns false if the Boid should be removed.
    public bool Evaluate()
    {
        if (currentEvaluation < 1f)
        {
            if (TimeMult > 0)
                currentEvaluation += Time.deltaTime / TimeMult;
            else
                currentEvaluation = 1f;

            //Evaluate the Curve
            currentValue = BoopCurve.Evaluate(currentEvaluation) * ValueMult;

            //If we're at the Infection percent through the curve (on a scale from 0 to 1) and we're not already decaying, then spread the infection and start to decay.
            if (decaying == false && currentEvaluation >= InfectionPercent)
            {
                decaying = true;
                ParentBoid.SpreadInfection(this);
            }
        }
        else
        {
            currentValue = 0f;
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
   
    //Active Boids can spread and receive infections. Reactive boids can receive infections but not spread them, Dead boids can't do anything.
    public enum BoidType { Active, Reactive, Dead }

    [Header("Boid Stats")]
    public BoidType BoidState = BoidType.Active;
    public bool IsTreasure = false;
    public bool RandomizeRotation = false;
    public Transform BoidVisual;
    public SpriteColorChanger BoidColor;

    [SerializeField]
    public BoopData DefaultBoop;

    public UnityEvent OnInfected;

    //Private Variables
    List<Boid> neighbors = new List<Boid>();
    List<BoopData> activeBoops = new List<BoopData>();

    float totalValue = 0f;
    public float Value { get { return totalValue; } }

    void Start()
    {
        if (RandomizeRotation)
        {
            float randomRot = Random.Range(0f, 360f);
            BoidVisual.Rotate(0f, 0f, randomRot);
        }
        BoidManager.current.OnTreasurePinged.AddListener(CheckPingedTreasure);
    }

    void Update()
    {
        if (EvaluateBoops())
        {
            BoidVisual.transform.localScale = Vector3.one * (1 + totalValue);
            BoidColor.SetValue(totalValue);
        }
    }

    void CheckPingedTreasure(int boopID)
    {
        int listPos = ContainsBoop(boopID);
        if (listPos != -1)
        {
            activeBoops[listPos].IsTreasureBoop = true;

            GiveRedStrengthFrom(activeBoops[listPos]);
        }
    }

    void GiveRedStrengthFrom(BoopData data)
    {
        float strength = 1 - data.CurrentTime;
        BoidColor.PingRed(strength);
    }

    bool EvaluateBoops()
    {
        if (activeBoops.Count > 0)
        {
            totalValue = 0f;

            for (int i = activeBoops.Count - 1; i >= 0; i--)
            {
                bool stillActive = activeBoops[i].Evaluate();
                totalValue += activeBoops[i].CurrentValue;

                if (!stillActive)
                    activeBoops.RemoveAt(i);
            }
            return true;
        }
        return false;
    }

    public void SpreadInfection(BoopData data)
    {
        if (BoidState == BoidType.Active)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                neighbors[i].Infect(data);
            }
        }
    }

    public void Infect(BoopData data)
    {
        if (BoidState == BoidType.Dead)
            return;
        
        if (ContainsBoop(data.BoopID) == -1 && data.ValueMult - data.GenerationalDecay > 0f)
        {
            BoopData newInfection = new BoopData(data, false);
            newInfection.ValueMult -= data.GenerationalDecay;
            newInfection.GenerationalDecay += data.AdditiveDecay;
            newInfection.ParentBoid = this;
            activeBoops.Add(newInfection);
            OnInfected.Invoke();

            if(IsTreasure)
                BoidManager.current.TreasurePinged(data.BoopID);

            if (data.IsTreasureBoop)
                GiveRedStrengthFrom(data);
        }
    }

    public int ContainsBoop(int boopID)
    {
        for (int i = 0; i < activeBoops.Count; i++)
        {
            if (activeBoops[i].BoopID == boopID)
                return i;
        }
        return -1;
    }

    [ContextMenu("Create Boop")]
    public void TestBoop()
    {
        if (BoidState == BoidType.Dead)
            return;
        
        BoopData newInfection = new BoopData(DefaultBoop, true);
        newInfection.ParentBoid = this;
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
