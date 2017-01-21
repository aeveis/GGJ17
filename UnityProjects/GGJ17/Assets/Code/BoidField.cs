using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidField : MonoBehaviour {

	//gameObject Boid;
	public GameObject boidPrototype;

	// attributes of field boids
	public float ScreenWidth = 16f;
	public float ScreenHeight = 10f;
	public float DistanceBetweenBoids = .2f;
	float totalBoids = 100;

	// Use this for initialization
	void Start () {


		for (var i = - ScreenWidth/2; i < ScreenWidth/2; i += DistanceBetweenBoids) 
		{
			for (var ii = - ScreenHeight/2; ii < ScreenHeight / 2; ii += DistanceBetweenBoids) 
			{
				GameObject go = Instantiate (boidPrototype);
				go.transform.SetParent (transform);
				go.transform.position = new Vector3 (i, ii, 0);
			}
		}


	}

}
