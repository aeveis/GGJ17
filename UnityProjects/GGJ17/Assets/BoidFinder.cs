using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BoidFinder : MonoBehaviour 
{
    List<Boid> nearbyBoids = new List<Boid>();

    public Boid GetClosestBoid()
    {
        Debug.Log("Getting Closest boid with " + nearbyBoids.Count + "nearby boids.");
        Boid bestBoid = null;
        float bestDistance = -1f;

        if (nearbyBoids.Count > 0)
        {
            bestBoid = nearbyBoids[0];
            if (nearbyBoids.Count == 1)
                return bestBoid;
            
            bestDistance = Vector3.Distance(transform.position, nearbyBoids[0].transform.position);
        }
        else
            return null;
        
        for (int i = 1; i < nearbyBoids.Count; i++)
        {
            float testDistance = Vector3.Distance(transform.position, nearbyBoids[i].transform.position);
            if (testDistance < bestDistance)
            {
                bestDistance = testDistance;
                bestBoid = nearbyBoids[i];
            }
        }
        Debug.Log("Returning " + bestBoid.gameObject.name + " as closest Boid.");
        return bestBoid;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Boid newNeighbor = col.GetComponent<Boid>();

        if (newNeighbor && nearbyBoids.Contains(newNeighbor) == false)
            nearbyBoids.Add(newNeighbor);
    }
}
