using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidRemover : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.SetActive(true);
    }
}
