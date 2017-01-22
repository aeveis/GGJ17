using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFX : MonoBehaviour {

    public GameObject CommPos;
    public GameObject CommNeg;
    public GameObject CommCrane;

    public Transform CranePool;

    public void ClickFX(bool Success)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f))
        {

            if(Success)
            {
                CommPos.transform.position = hit.point;
                CommPos.SetActive(false);
                CommPos.SetActive(true);
            }
            else
            {
                CommNeg.transform.position = hit.point;
                CommNeg.SetActive(false);
                CommNeg.SetActive(true);
            }

        }
    }

    public void CraneFX(Vector3 Position, bool Success)
    {
        GameObject crane = GameObject.Instantiate(CommCrane, CranePool);
        crane.transform.position = Position;

        if(Success)
        {
            crane.GetComponent<Animator>().SetTrigger("Success");
        }
        else
        {
            crane.GetComponent<Animator>().SetTrigger("Fail");
        }
    }

    public void CleanUpCranePool()
    {
        for(int i = CranePool.childCount-1;i>-1; i--)
        {
            GameObject.Destroy(CranePool.GetChild(i).gameObject);
        }
    }
}
