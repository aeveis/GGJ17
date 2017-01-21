using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [Header("Gizmos Parameters")]
    public float GizmoRadius = 1f;
    public Color GizmoColor = Color.red;

    [Header("Visual Controls")]
    public GameObject TreasureVisual;
    public bool isFound = false;

    private void Start()
    {
        TreasureVisual.SetActive(false);
    }

    private void OnMouseOver()
    {
        Debug.Log("Over");
        if(Input.GetMouseButtonUp(1))
        {
            Debug.Log("Treasure get!");
            isFound = true;
            TreasureVisual.SetActive(true);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawSphere(transform.position, GizmoRadius);
        
    }

}
