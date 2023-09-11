using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public Image Image;
    public Text Label;

    private int _count = 0;
    private float _startTime = -1;

    public void Open(Action callback = null)
    {
        StartCoroutine(Loading(callback));
        _startTime = Time.time;
    }

    public void Close()
    {
        Image.gameObject.SetActive(false);
        _startTime = -1;
    }

    public IEnumerator Loading(Action callback)
    {
        Image.gameObject.SetActive(true);

        yield return null;

        if (callback != null)
        {
            callback();
        }
    }

    private void Update()
    {
        if (_startTime != -1 && (Time.time - _startTime) > 0.1f)
        {
            string text = "Loading.";
            for (int i = 0; i < _count; i++)
            {
                text += ".";
            }
            Label.text = text;
            _count = (_count + 1) % 3;
            _startTime = Time.time;
        }
    }

    void Awake()
    {
    }
}
