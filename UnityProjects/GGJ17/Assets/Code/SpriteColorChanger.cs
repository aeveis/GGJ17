using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteColorChanger : MonoBehaviour {

    public Gradient WaveGradient;
    public float ColorMult = 1f;

    SpriteRenderer render;

    float value = 0f;
    float prevValue = -1f;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();

    }

    public void SetValue(float newValue)
    {
        value = newValue;
    }

    void Update()
    {
        if (value != prevValue)
        {
            render.color = WaveGradient.Evaluate(value * ColorMult + 0.5f);
            prevValue = value;
        }
    }
}
