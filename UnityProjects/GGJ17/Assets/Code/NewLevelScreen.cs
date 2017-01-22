using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewLevelScreen : MonoBehaviour 
{
    public Text LevelText;
    public Text ChestText;
    public Animator animator;

    public void SetShow(bool show)
    {
        animator.SetBool("Show", show);
    }

    public void Configure(string levelName, int maxChests)
    {
        LevelText.text = levelName;
        ChestText.text = "Chests: 0/" + maxChests;
    }
}
