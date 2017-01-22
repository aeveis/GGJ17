using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CoinUI : MonoBehaviour {

    public Image ImageToSwap;
    public Sprite SwappedGraphic;
    public UnityEvent OnSwapped;

    public bool Swapped = false;

    [ContextMenu("Test Swap")]
    public void Swap()
    {
        Swapped = true;
        ImageToSwap.sprite = SwappedGraphic;
        OnSwapped.Invoke();
    }
}
