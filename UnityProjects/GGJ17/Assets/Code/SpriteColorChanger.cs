using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteColorChanger : MonoBehaviour {

    public Gradient WaveGradient;
    public Color RedColor = Color.red;
    public AnimationCurve RedBlendWeightOverTime;
    public float ColorMult = 1f;
    public float RedDecayMult = 1f;

    SpriteRenderer render;

    float value = 0f;
    float prevValue = -1f;

    float currentRedPercent = 0f;
    float currentRedValue = 0f;
    float prevRedValue;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();

    }

    public void PingRed(float falloff)
    {
        currentRedPercent += Mathf.Clamp(1 - falloff, 0f, 1f);
    }

    public void SetValue(float newValue)
    {
        value = newValue;
    }

    void Update()
    {
        if (value != prevValue || currentRedValue != prevRedValue)
        {
            Color waveColor = WaveGradient.Evaluate(value * ColorMult + 0.5f);
            render.color = Color.Lerp(waveColor, RedColor, currentRedValue);

            prevValue = value;
            prevRedValue = currentRedValue;
        }

        if (currentRedPercent > 0)
        {
            currentRedPercent -= Time.deltaTime * RedDecayMult;
            currentRedValue = RedBlendWeightOverTime.Evaluate(currentRedPercent);
        }
        else if (currentRedValue != 0)
            currentRedValue = 0f;
    }
}
