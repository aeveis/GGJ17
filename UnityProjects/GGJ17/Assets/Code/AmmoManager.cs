using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AmmoManager : MonoBehaviour {
    [Header("UI Prefabs")]
    public GameObject guessUIPrefab;
    public GameObject guessUIParent;
    public GameObject coinUIPrefab;
    public Sprite coinUIFound;
    public Sprite coinUIHidden;
    public GameObject coinUIParent;
    public float guessYOffset = 10f;

    [Header("Initial Ammo")]
    public int initialBoops = 5;
    public int initialGuesses = 3;

    [Header("Current Ammo Levels")]
    public int boopsRemaining;
    public int guessesRemaining;
    private List<GameObject> guessUIList = new List<GameObject>();

    [Header("Treasure Data")]
    public LayerMask TreasureCollisionLayer;
    public GameObject debugSphere;

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

    private void SpawnCoinUIs()
    {

    }

    /* Update */
    private void Update()
    {
        if (Input.GetMouseButtonUp(1) && !(MetaScreen.current.isPaused))
        {
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
                    }
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
