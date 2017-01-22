using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBumper : MonoBehaviour 
{
    public float Cooldown = 1f;
    public List<string> CollidesWithTags = new List<string>();
    public float MinCollisionForce = 2f;
    public float MaxCollisionForce = 3f;

    public float BumperForce = 2f;

    float currentCooldown = 0f;

    void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (currentCooldown <= 0)
        {
            for (int i = 0; i < CollidesWithTags.Count; i++)
            {
                //Debug.Log("SUB COLLISION DETECTED KIND OF. Dected Collision with "+col.gameObject.tag + " on "+col.gameObject.name);
                if (col.gameObject.tag == CollidesWithTags[i])
                {
                    //Debug.Log("----> SUB Collision Velocity: " + col.relativeVelocity.magnitude);
                    if (col.relativeVelocity.magnitude >= MinCollisionForce && col.relativeVelocity.magnitude <= MaxCollisionForce)
                    {
                        currentCooldown = Cooldown;
                        //DO SOMETHING

                        Vector2 normal = col.contacts[0].normal;
                        col.collider.attachedRigidbody.AddForce(normal.normalized * -1f * BumperForce, ForceMode2D.Force);
                    }
                }
            }

        }
    }
}
