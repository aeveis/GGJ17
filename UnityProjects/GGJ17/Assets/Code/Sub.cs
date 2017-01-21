using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Sub : MonoBehaviour {

	public Vector2 ForceRandomness = Vector2.zero;
	public float ForceMult = 1f;
	public float MaxDistance = 5f;
	public AnimationCurve ForceFalloff;

	Rigidbody2D subBody;
	List<Boid> neighbors = new List<Boid>();

	void Start(){
		transform.localPosition = new Vector3 (0, 0, 0);
		subBody = GetComponent<Rigidbody2D> ();
	}

	void ApplyForceToMeFrom (Vector2 sourcePos)
	{
		float distance = Vector2.Distance (sourcePos, subBody.position);
		float forceMag = ForceFalloff.Evaluate (distance / MaxDistance) * ForceMult;

		Vector2 direction = subBody.position - sourcePos;
		direction = direction.normalized;

		Vector2 forcePos;
		forcePos.x = Random.Range (ForceRandomness.x * -1, ForceRandomness.x);
		forcePos.y = Random.Range (ForceRandomness.y * -1, ForceRandomness.y);

		subBody.AddForceAtPosition (direction * forceMag, forcePos); 
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
