using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Sub : MonoBehaviour 
{
    [Header("Handling Characteristics")]
	public float ForceMult = 1f;
	public float MaxDistance = 5f;
	public AnimationCurve ForceFalloff;
    public float MaxWaveAge = 2f;
    public AnimationCurve ForceDecayToAge;

    [Header("Health Characteristics")]
    public ParticleSystem Smoke;
    public int MaxHp = 3;
    public bool Invulnerable = false;
    public UnityEvent OnReset;
    public UnityEvent OnDamaged;
    public UnityEvent OnDestroyed;

    int currentHp;
	Rigidbody2D subBody;
	List<Boid> neighbors = new List<Boid>();

	void Start(){
		transform.localPosition = new Vector3 (0, 0, 0);
		subBody = GetComponent<Rigidbody2D> ();
        currentHp = MaxHp;
	}

    public void ResetHealth()
    {
        SetSmokeEmission(0f);
        currentHp = MaxHp;
        OnReset.Invoke();
    }

    public void TakeDamage()
    {
        if (!Invulnerable)
        {
            currentHp -= 1;

            if (currentHp <= 0)
            {
                bool didRestart = GameManager.current.ResetLevel();
                if (didRestart)
                {
                    SetSmokeEmission(0f);
                    OnDestroyed.Invoke();
                }
                else
                    ResetHealth(); // Some Race condition safety. If we died while transitioning, just reset the sub.
            }
            else
            {
                SetSmokeEmission(5f * (MaxHp - currentHp));
                OnDamaged.Invoke();
            }
        }
    }

    public void SetSmokeEmission(float newRate)
    {

        StartCoroutine(SetSmokeEmissionCheck(newRate));
       // var em = Smoke.emission;
        
        // var rate = em.rateOverTime;

        //   rate.constant = newRate;

        //   em.rateOverTime = rate;
    }

    private IEnumerator SetSmokeEmissionCheck(float newRate)
    {
        while (Smoke==null|| !Smoke.gameObject.activeSelf || !Smoke.emission.enabled)
        {
            yield return null;
        }

        var em = Smoke.emission.rateOverTime;
        em.mode = ParticleSystemCurveMode.Constant;
        em.constantMax = newRate;
        em.constantMin = newRate;
    }

    public void ForceToPosition(Vector3 position)
    {
        transform.position = position;
        subBody.velocity = Vector2.zero;
        subBody.angularVelocity = 0f;
    }

    void ApplyForceToMeFrom (BoopData data)
	{
        Vector2 sourcePos = new Vector2(data.ParentBoid.transform.position.x, data.ParentBoid.transform.position.y);

		float distance = Vector2.Distance (sourcePos, subBody.position);
        float forceMag = ForceFalloff.Evaluate(distance / MaxDistance) * ForceMult;

        float additionalForce = ForceDecayToAge.Evaluate(data.BoopAge / MaxWaveAge);

        forceMag *= additionalForce;

		//Vector2 direction = subBody.position - sourcePos;
		//direction = direction.normalized;
        Vector2 direction = data.Direction;

        //subBody.AddForceAtPosition (direction * forceMag, forcePos, ForceMode2D.Force); 
        subBody.AddForce( (direction) * forceMag, ForceMode2D.Force);
        subBody.AddTorque( Random.Range(forceMag * -1 / 10f, forceMag / 10f), ForceMode2D.Force);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Boid newNeighbor = col.GetComponent<Boid>();

		if (newNeighbor && neighbors.Contains (newNeighbor) == false) {
			neighbors.Add (newNeighbor);
			newNeighbor.OnInfected.AddListener (ApplyForceToMeFrom);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		Boid oldNeighbor = col.GetComponent<Boid>();

		if (oldNeighbor) {
			oldNeighbor.OnInfected.RemoveListener (ApplyForceToMeFrom);
			neighbors.Remove (oldNeighbor);
		}
	}

}
