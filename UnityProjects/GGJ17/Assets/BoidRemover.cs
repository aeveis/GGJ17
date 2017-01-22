using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidRemover : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        BoidWrapper boidWrapper = other.gameObject.GetComponent<BoidWrapper>();
        if(boidWrapper)
        {
            boidWrapper.boidInfo.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BoidWrapper boidWrapper = other.gameObject.GetComponent<BoidWrapper>();
        if (boidWrapper)
        {
            boidWrapper.boidInfo.gameObject.SetActive(true);
        }
    }

    private void OnMouseDown()
    {

        GameManager.current.CommFX.ClickFX(false);
    }
}
