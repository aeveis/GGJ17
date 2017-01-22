using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Sub : MonoBehaviour 
{
	public float ForceMult = 1f;
	public float MaxDistance = 5f;
	public AnimationCurve ForceFalloff;
    public float MaxWaveAge = 2f;
    public AnimationCurve ForceDecayToAge;

	Rigidbody2D subBody;
	List<Boid> neighbors = new List<Boid>();

	void Start(){
		transform.localPosition = new Vector3 (0, 0, 0);
		subBody = GetComponent<Rigidbody2D> ();
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
