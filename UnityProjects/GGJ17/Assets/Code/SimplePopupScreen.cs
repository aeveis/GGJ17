using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimplePopupScreen : MonoBehaviour 
{
    public Animator animator;

    public void SetShow(bool show)
    {
        animator.SetBool("Show", show);
    }
}
