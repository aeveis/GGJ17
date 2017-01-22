using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SimplePopupScreen : MonoBehaviour 
{
    public Animator animator;
    public UnityEvent OnShown;


    public void SetShow(bool show)
    {
        if (show)
            OnShown.Invoke();
        
        animator.SetBool("Show", show);
    }
}
