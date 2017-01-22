using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[System.Serializable]
public class LevelInfo
{
    public string LevelID = "Enter Scene Name Here.";
    public int ChestsToComplete = 1;
}

public class MetaScreen : MonoBehaviour {
    public static MetaScreen current;

    [Header("Managers")]
    public SubSpawn SubSpawner;
    public BoidField BoidSpawner;
    public AmmoManager HUDManager;

    [Header("All Levels Info")]
    public List<LevelInfo> LevelList;

    [Header("Current Level Info")]
    public int CurrentLevel = 0;
    public bool CurrentLevelComplete = false;

    public int TreasureCollected = 0;

    [Header("Menu Screens")]
    public GameObject PauseScreen;

    public Animator Fader;

    [Header("Menu Bools")]
    private bool isInMenuOverlay = false;

    private void Awake()
    {
        current = this;
        PauseScreen.SetActive(false);
        BoidSpawner.Generate();
    }

    public LevelInfo GetCurrentLevelInfo()
    {
        return LevelList[CurrentLevel];
    }

    private void Start()
    {
        SceneManager.LoadScene(LevelList[CurrentLevel].LevelID, mode: LoadSceneMode.Additive);
        SubSpawner.SpawnSubAt(Vector3.zero);
    }

    private void AdvanceLevel()
    {
        if (CurrentLevel < LevelList.Count - 1)
        {
            CurrentLevel++;
            SceneManager.UnloadSceneAsync(LevelList[CurrentLevel - 1].LevelID);
            SceneManager.LoadScene(LevelList[CurrentLevel].LevelID, mode: LoadSceneMode.Additive);
            SubSpawner.ResetSub();
        }
        else
        {
            Debug.Log("You win! No more levels left!");
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
            AdvanceLevel();
        }
    }

    public void CollectATreasure ()
    {
        Debug.Log("Treasure collected!");
        TreasureCollected += 1;
        CheckIfLevelComplete();
    }

    public void CheckIfLevelComplete ()
    {
        if(TreasureCollected >= GetCurrentLevelInfo().ChestsToComplete)
        {
            Debug.Log("You won the level!");
            AdvanceLevel();
        }
    }

    private void FadeToBlack(bool isAtBlack)
    {
        Debug.Log("Fading: " + isAtBlack);
        Fader.SetBool("FadeTowardsBlack", isAtBlack);
    }

    public void QuitAnimation ()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }

}
