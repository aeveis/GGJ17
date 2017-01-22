using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HUDManager : MonoBehaviour {
    [Header("UI Prefabs")]
    public GameObject guessUIPrefab;
    public GameObject guessUIParent;
    public GameObject coinUIPrefab;
    public Sprite coinUIFound;
    public Sprite coinUIHidden;
    public GameObject coinUIParent;
    public float guessYOffset = 10f;
    public float coinXOffset = 10f;
    public float coinYOffset = 50f;

    [Header("Initial Ammo")]
    public int initialBoops = 5;
    public int initialGuesses = 3;
    public int maxGuesses = 8;

    [Header("Current Ammo Levels")]
    public int boopsRemaining;
    public int guessesRemaining;
    private List<GameObject> guessUIList = new List<GameObject>();
    private List<GameObject> coinUIList = new List<GameObject>();

    private void Start()
    {
        SpawnCraneUIs();
        SpawnCoinUIs();
    }

    /* UI Spawners */
    private void SpawnCraneUIs()
    {
        float guessPrefabHeight = guessUIPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < initialGuesses; i++)
        {
            GameObject nextIcon = Instantiate(guessUIPrefab) as GameObject;
            guessUIList.Add(nextIcon);
            nextIcon.transform.SetParent(guessUIParent.transform, false);

            float newY = transform.parent.localPosition.y - (guessYOffset + guessPrefabHeight) * i;
            nextIcon.transform.localPosition = new Vector3(0, newY, 0);
        }

        boopsRemaining = initialBoops;
        guessesRemaining = initialGuesses;
    }

    public void AddUpToCraneUIs(int amount)
    {
        int totalGuesses = amount>maxGuesses?maxGuesses:amount;

        float guessPrefabHeight = guessUIPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = guessesRemaining; i < totalGuesses; i++)
        {
            GameObject nextIcon = Instantiate(guessUIPrefab) as GameObject;
            guessUIList.Add(nextIcon);
            nextIcon.transform.SetParent(guessUIParent.transform, false);

            float newY = transform.parent.localPosition.y - (guessYOffset + guessPrefabHeight) * i;
            nextIcon.transform.localPosition = new Vector3(0, newY, 0);
        }
        
        guessesRemaining = totalGuesses;
    }

    private void SpawnCoinUIs()
    {
        float coinPrefabHeight = coinUIPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < GameManager.current.GetCurrentLevelInfo().ChestsToComplete; i++)
        {
            GameObject nextIcon = Instantiate(coinUIPrefab) as GameObject;
            coinUIList.Add(nextIcon);
            nextIcon.transform.SetParent(coinUIParent.transform, false);

            float newX = (transform.parent.localPosition.x + coinXOffset) - (coinXOffset + coinPrefabHeight) * i;
            float newY = coinYOffset;
            nextIcon.transform.localPosition = new Vector3(newX, newY, 0);
        }

    }

    /* Update */
    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Got Click Up.");
            if(guessUIList.Count > 0)
            {
                Debug.Log("Fire!");
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.gameObject.name + " Target Position: " + hit.collider.gameObject.transform.position);
                    Treasure myTreasure = hit.collider.gameObject.GetComponent<Treasure>();
                    if (myTreasure)
                    {
                        myTreasure.SetTreasureFound();
                        GameManager.current.CommFX.CraneFX(hit.point, true);
                    }
                    else
                    {
                        GameManager.current.CommFX.CraneFX(hit.point, false);
                    }
                }
                else
                {
                    Debug.Log("Somehow you hit nothing");
                    return;
                }

                RemoveFireUIHandler();
            }

        }
    }

    private void RemoveFireUIHandler()
    {
        GameObject removingThisUI = guessUIList[guessUIList.Count - 1];
        guessUIList.Remove(removingThisUI);
        Destroy(removingThisUI);

        guessesRemaining -= 1;
        if (guessesRemaining == 0)
        {
            Debug.Log("Game over");
        }
    }

}
