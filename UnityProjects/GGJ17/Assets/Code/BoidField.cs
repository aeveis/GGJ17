using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidField : MonoBehaviour {

	public enum GenerationType { Grid, Cluster, RandomGrid }

	public GenerationType GenerationMethod = GenerationType.Grid;

	//gameObject Boid;
	public GameObject boidPrototype;

	// attributes of field boids
	public float ScreenWidth = 16f;
	public float ScreenHeight = 10f;
	public float DistanceBetweenBoids = .5f;
	float totalBoids = 100;
	public float randomSpacingThreshold = .05f;

	// Use this for initialization
	void Start () {

		switch (GenerationMethod) 
		{
		case GenerationType.Grid:
			grid ();
			break;
		case GenerationType.Cluster:
			cluster ();
			break;
		case GenerationType.RandomGrid:
			randomGrid ();
			break;
		default:
			randomGrid ();
			break;
		}
			
	}
		
	void grid(){
		DistanceBetweenBoids = .2f;
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

	void cluster(){
		DistanceBetweenBoids = .5f;
		for (var i = - ScreenWidth/2; i < ScreenWidth/2; i += DistanceBetweenBoids) 
		{
			for (var ii = - ScreenHeight/2; ii < ScreenHeight / 2; ii += DistanceBetweenBoids) 
			{
				GameObject go = Instantiate (boidPrototype);
				go.transform.SetParent (transform);
				go.transform.position = new Vector3 (i*Random.value, ii*Random.value, 0);
			}
		}
	}

	void randomGrid(){
		DistanceBetweenBoids = .25f;
		randomSpacingThreshold = .05f;

		for (var i = - ScreenWidth/2; i < ScreenWidth/2; i += DistanceBetweenBoids) 
		{
			for (var ii = - ScreenHeight/2; ii < ScreenHeight / 2; ii += DistanceBetweenBoids) 
			{
				GameObject go = Instantiate (boidPrototype);
				go.transform.SetParent (transform);
				go.transform.position = new Vector3 (i + DistanceBetweenBoids * Random.value - randomSpacingThreshold, ii + DistanceBetweenBoids * Random.value - randomSpacingThreshold, 0);
			}
		}
	}

}

