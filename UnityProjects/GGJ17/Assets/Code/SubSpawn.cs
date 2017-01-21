using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSpawn : MonoBehaviour {

	//gameObject Boid;
	public GameObject subPrototype;

	// Use this for initialization
	void Start () {
		GameObject go = Instantiate (subPrototype);
		go.transform.SetParent (transform);
		go.transform.position = new Vector3 (0, 0, 0);
	}

}
