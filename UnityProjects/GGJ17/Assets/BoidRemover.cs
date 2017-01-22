using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidRemover : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        BoidWrapper boidWrapper = other.gameObject.GetComponent<BoidWrapper>();
        if(boidWrapper)
        {
            boidWrapper.boidInfo.BoidState = Boid.BoidType.Dead;
            boidWrapper.boidInfo.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BoidWrapper boidWrapper = other.gameObject.GetComponent<BoidWrapper>();
        if (boidWrapper)
        {
            Debug.Log("everyone get out");
            boidWrapper.boidInfo.gameObject.SetActive(true);
            boidWrapper.boidInfo.BoidState = Boid.BoidType.Active;
        }
    }

    private void OnMouseDown()
    {

        GameManager.current.CommFX.ClickFX(false);
    }
}
