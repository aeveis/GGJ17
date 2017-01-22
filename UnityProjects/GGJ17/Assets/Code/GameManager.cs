using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[System.Serializable]
public class LevelInfo
{
    public string LevelID = "Enter Scene Name Here.";
    public string PlayerFacingName = "A snappy Name";
    public int InitialCranes = 3;
    public int ChestsToComplete = 1;
}

public class GameManager : MonoBehaviour {
    public static GameManager current;

    [Header("Managers")]
    public SubSpawn SubSpawner;
    public BoidField BoidSpawner;
    public HUDManager HUDManager;
    public CommandFX CommFX;

    [Header("All Levels Info")]
    public List<LevelInfo> LevelList;

    [Header("Current Level Info")]
    public int CurrentLevel = 0;
    public int TreasureCollected = 0;

    [Header("Menu Screens")]
    public GameObject PauseScreen;
    public NewLevelScreen NextLevelScreen;
    public SimplePopupScreen CompleteScreen;
    public SimplePopupScreen GameWinScreen;
    public SimplePopupScreen ControlsScreen;
    public SimplePopupScreen LevelSelectScreen;

    [Header("Events")]
    public UnityEvent OnLevelSuccess;

    public Animator Fader;

    [Header("Menu Bools")]
    private bool isInMenuOverlay = false;

    //Private variables
    bool currentLevelComplete = false;

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
        StartCoroutine(FirstLevelCoroutine());
    }

    IEnumerator FirstLevelCoroutine()
    {
        NextLevelScreen.Configure(GetCurrentLevelInfo().PlayerFacingName, GetCurrentLevelInfo().ChestsToComplete);
        NextLevelScreen.SetShow(true);
        yield return new WaitForSeconds(4f);
        NextLevelScreen.SetShow(false);
    }

    private void AdvanceLevel()
    {
        //If the current level we're in is already complete, wait.
        if (currentLevelComplete)
            return;
        
        CurrentLevel++;

        if (CurrentLevel <= LevelList.Count - 1)
        {
            currentLevelComplete = true;
            StartCoroutine(AdvanceLevelCoroutine());
        }
        else
        {
            Debug.Log("You win! No more levels left!");
            currentLevelComplete = true;
            SetFadeState(true);
            GameWinScreen.SetShow(true);
        }
    }

    public bool ResetLevel()
    {
        if (!currentLevelComplete)
        {
            StartCoroutine(ResetLevelCoroutine());
            return true;
        }
        else
            return false;
    }

    IEnumerator ResetLevelCoroutine()
    {
        currentLevelComplete = true;
        yield return new WaitForSeconds(1f);
        SetFadeState(true);
        CommFX.CleanUpCranePool();
        HUDManager.AddUpToCraneUIs(GetCurrentLevelInfo().InitialCranes);
        HUDManager.SpawnCoinUIs();
        yield return new WaitForSeconds(1f);
        SceneManager.UnloadSceneAsync(LevelList[CurrentLevel].LevelID);
        SceneManager.LoadScene(LevelList[CurrentLevel].LevelID, mode: LoadSceneMode.Additive);
        TreasureCollected = 0;
        SubSpawner.ResetSub();
        SetFadeState(false);
        yield return new WaitForSeconds(1f);
        currentLevelComplete = false;
    }

    IEnumerator AdvanceLevelCoroutine()
    {
        //TODO: End of Level Show
        yield return new WaitForSeconds(1f);
        SetFadeState(true);
        CompleteScreen.SetShow(true);
        OnLevelSuccess.Invoke();
        CommFX.CleanUpCranePool();
        HUDManager.AddUpToCraneUIs(GetCurrentLevelInfo().InitialCranes);


        yield return new WaitForSeconds(1f);
        //move obstacles so on trigger exit is called - Dan
        var obstacles = GameObject.FindObjectsOfType<BoidRemover>();
        for (int i = 0; i < obstacles.Length; i++)
        {
            Debug.Log("obstacles: " + obstacles[i]);
            obstacles[i].gameObject.transform.position = Vector3.one * -5f;
        }
        yield return new WaitForSeconds(1f);

        CompleteScreen.SetShow(false);
        SetFadeState(false);
        HUDManager.SpawnCoinUIs();
        SceneManager.UnloadSceneAsync(LevelList[CurrentLevel - 1].LevelID);
        SceneManager.LoadScene(LevelList[CurrentLevel].LevelID, mode: LoadSceneMode.Additive);
        TreasureCollected = 0;
        SubSpawner.ResetSub();

        yield return new WaitForSeconds(1f);
        NextLevelScreen.Configure(GetCurrentLevelInfo().PlayerFacingName, GetCurrentLevelInfo().ChestsToComplete);
        NextLevelScreen.SetShow(true);
        yield return new WaitForSeconds(3f);
        NextLevelScreen.SetShow(false);
        currentLevelComplete = false;

    }

    public void TogglePauseMenu()
    {
        isInMenuOverlay = !isInMenuOverlay;
        if (isInMenuOverlay) { PauseScreen.SetActive(true); }
        else { PauseScreen.SetActive(false); }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            AdvanceLevel();
        }

        if(Input.GetKeyUp(KeyCode.R))
        {
            ResetLevel();
        }
    }

    public void CollectATreasure ()
    {
        Debug.Log("Treasure collected!");
        TreasureCollected += 1;
        HUDManager.GetNextCoin();
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

    private void SetFadeState(bool fade)
    {
        Fader.SetBool("FadeTowardsBlack", fade);
    }

    /* Pause Menu Buttons */

    public void RestartLevel ()
    {
        Debug.Log("Restarting...");
        TogglePauseMenu();
        ResetLevel();
    }

    public void ShowLevelSelect ()
    {

    }

    public void ShowControls ()
    {

    }

    public void HideControls ()
    {

    }

    public void QuitAnimation ()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }

}
