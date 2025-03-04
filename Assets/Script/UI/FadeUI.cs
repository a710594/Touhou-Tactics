using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class FadeUI : MonoBehaviour
{
    public float Time;
    public Image Image;

    public void Open(Action callback) 
    {
        StartCoroutine(Fade(callback));
    }

    public void Close() 
    {
        Image.DOFade(0, Time);
    }

    public IEnumerator Fade(Action callback)
    {
        Image.DOFade(1, Time).OnComplete(()=> 
        {
            if (callback != null)
            {
                callback();
            }
        });

        yield return null;
    }
}
