using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollisionDetection : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Treasure"))
        {
            Debug.Log("Treasure get!");
        }
        else if(collision.gameObject.CompareTag("Dangerous"))
        {
            Debug.Log("You dead.");
        }
    }
}
