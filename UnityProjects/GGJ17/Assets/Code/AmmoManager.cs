using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour {
    [Header("UI Prefabs")]
    public GameObject guessUIPrefab;
    public GameObject guessUIParent;
    public float guessXOffset = 10f;

    [Header("Initial Ammo")]
    public int initialBoops = 5;
    public int initialGuesses = 3;

    [Header("Current Ammo Levels")]
    public int currentBoopsRemaining;
    public int currentGuessesRemaining;

    private void Start()
    {
        float guessPrefabWidth = guessUIPrefab.GetComponent<RectTransform>().rect.width;
        for(int i = 0; i < initialGuesses; i++)
        {
            GameObject nextIcon = Instantiate(guessUIPrefab) as GameObject;
            nextIcon.transform.SetParent(guessUIParent.transform, false);

            float newX = transform.parent.localPosition.x + (guessXOffset + guessPrefabWidth)* i;
            nextIcon.transform.localPosition = new Vector3(newX, 0, 0);
        }
    }

}
