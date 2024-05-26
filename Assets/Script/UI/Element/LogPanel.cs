using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LogPanel : MonoBehaviour
{
    public Text Label;
    public CanvasGroup CanvasGroup;

    private Timer _timer = new Timer();

    public void SetLabel(string text)
    {
        Label.text = text;
    }

    public void Fade(float time, Action<LogPanel> callback)
    {
        _timer.Start(time, ()=>
        {
            CanvasGroup.DOFade(0, 1).OnComplete(()=>
            {
                CanvasGroup.alpha = 1;
                gameObject.SetActive(false);
                callback(this);
            });
        });
    }

    void Awake()
    {
        gameObject.SetActive(false);
    }
}
