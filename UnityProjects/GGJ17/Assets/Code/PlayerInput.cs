using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent onBooped;

    private void OnMouseDown()
    {
        onBooped.Invoke();
    }
}
