using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneFX : MonoBehaviour {

    public Sprite[] WrongSpotOptions;
    public SpriteRenderer WrongSpot;

    private void Start()
    {
        WrongSpot.sprite = WrongSpotOptions[(int)Mathf.Floor(((float)WrongSpotOptions.Length) * Random.Range(0f, .99f))];
    }
}
