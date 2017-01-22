using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleChest : MonoBehaviour {

    public Treasure chest;

	// Use this for initialization
	void Start () {
        StartCoroutine(chest.SetTreasureFoundDelayed());
        StartCoroutine(CheckBoid());
	}

    private IEnumerator CheckBoid()
    {
        while (chest.MyBoid == null)
        {
            yield return null;
        }
        chest.MyBoid.IsTreasure = false;
    }
}
