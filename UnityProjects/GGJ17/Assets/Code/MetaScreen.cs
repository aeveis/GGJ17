using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaScreen : MonoBehaviour {
    [Header("Level Info")]
    public int currentLevel;
    public int lastUnlockedLevel;

    [Header("Pause Handler")]
    public GameObject PauseScreen;
    private bool isInMenuOverlay = false;

    private void Start()
    {
        PauseScreen.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            isInMenuOverlay = !isInMenuOverlay;
            if (isInMenuOverlay) { PauseScreen.SetActive(true); }
            else { PauseScreen.SetActive(false); }
        }
    }
}
