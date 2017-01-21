using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialColorChanger : MonoBehaviour 
{
    Renderer render;
    Material instanceMat;

    void Awake()
    {
        render = GetComponent<Renderer>();
        instanceMat = render.material;
    }


}
