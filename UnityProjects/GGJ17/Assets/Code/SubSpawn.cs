using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSpawn : MonoBehaviour {

	//gameObject Boid;
    public Sub SubPrefab;

    Sub activeSub;
    Vector3 spawnPos;

    public Sub PlayerSub { get { return activeSub; } }

    public void SpawnSubAt(Vector3 spawnPosition)
    {
        GameObject go = Instantiate (SubPrefab.gameObject);
        go.transform.SetParent (transform);
        go.transform.position = spawnPosition;
        activeSub = go.GetComponent<Sub>();

        spawnPos = spawnPosition;
    }

    public void ResetSub()
    {
        activeSub.ForceToPosition(spawnPos);
        activeSub.ResetHealth();
    }

}
