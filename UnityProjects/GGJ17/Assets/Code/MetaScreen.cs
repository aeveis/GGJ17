using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaScreen : MonoBehaviour {
    [Header("Level Info")]
    public int currentLevel;
    public int lastUnlockedLevel;
    public bool currentLevelComplete = false;
    public List<string> allLevels;

    [Header("Pause Handler")]
    public GameObject PauseScreen;
    private bool isInMenuOverlay = false;

    private void Start()
    {
        PauseScreen.SetActive(false);
        LoadSceneNumber(1);
    }

    private void LoadSceneNumber(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber, mode: LoadSceneMode.Additive);
    }

    private void NextScene()
    {
        currentLevel++;
        SceneManager.UnloadSceneAsync(allLevels[currentLevel-1]);
        SceneManager.LoadScene(allLevels[currentLevel], mode: LoadSceneMode.Additive);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            isInMenuOverlay = !isInMenuOverlay;
            if (isInMenuOverlay) { PauseScreen.SetActive(true); }
            else { PauseScreen.SetActive(false); }
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            NextScene();
        }
    }
}
