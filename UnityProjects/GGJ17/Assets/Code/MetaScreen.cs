using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaScreen : MonoBehaviour {
    public static MetaScreen current;

    [Header("All Levels Info")]
    public int lastUnlockedLevel;
    public List<string> allLevels;

    [Header("Current Level Info")]
    public int currentLevel = 1;
    public bool currentLevelComplete = false;
    public Treasure[] allCurrentTreasure;
    public int currentTreasureFound = 0;

    [Header("Menu Screens")]
    public GameObject PauseScreen;
    public GameObject ControlsScreen;
    public GameObject LevelSelectScreen;

    public Animator BlackFader;
    public bool TempFaderBool = false;

    [Header("Menu Bools")]
    public bool isPaused = false;
    private bool isInMenuOverlay = false;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        PauseScreen.SetActive(false);
        LoadSceneNumber(currentLevel);
        GetAllChests();
    }

    private void LoadSceneNumber(int sceneNumber)
    {
        SceneManager.LoadScene(allLevels[sceneNumber], mode: LoadSceneMode.Additive);
    }

    private void NextScene()
    {
        if (currentLevel < allLevels.Count - 1)
        {
            isPaused = true;
            currentLevel++;
            SceneManager.UnloadSceneAsync(allLevels[currentLevel - 1]);
            SceneManager.LoadScene(allLevels[currentLevel], mode: LoadSceneMode.Additive);
            isPaused = false;
        }
        else
        {
            Debug.Log("You win!");
            //TODO: YOU WIN SCREEN
        }
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

        if(Input.GetKeyUp(KeyCode.B))
        {
            FadeToBlack(TempFaderBool);
            TempFaderBool = !TempFaderBool;
        }
    }

    private void GetAllChests ()
    {
        allCurrentTreasure = GameObject.FindObjectsOfType<Treasure>();
        Debug.Log(allCurrentTreasure.Length);
    }

    public void CollectATreasure (Treasure thisTreasure)
    {
        Debug.Log("Treasure collected!");
    }

    private void FadeToBlack(bool isAtBlack)
    {
        Debug.Log("Fading: " + isAtBlack);
        BlackFader.SetBool("FadeTowardsBlack", isAtBlack);
    }

    public void QuitAnimation ()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }

}
