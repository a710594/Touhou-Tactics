using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ValueBar : MonoBehaviour
{
    public Image Bar;
    public Text Label;

    private bool _isTweening = false;
    private int _maxHP;
    private Tweener _tweener;

    public void SetValue(int current, int max)
    {
        if (max != 0)
        {
            Bar.fillAmount = (float)current / (float)max;
        }
        else
        {
            Bar.fillAmount = 0;
        }
        Label.text = current.ToString() + "/" + max.ToString();
    }

    public void SetValueTween(int current, int max, Action callback)
    {
        _isTweening = true;
        _maxHP = max;
        if (max != 0)
        {
            _tweener = Bar.DOFillAmount((float)current / (float)max, 0.5f).OnComplete(() =>
            {
                if (callback != null)
                {
                    callback();
                }
            });
            _tweener.SetUpdate(true);
        }
        else
        {
            Bar.fillAmount = 0;
        }
    }

    public void SetValueTween(int from, int to, int max, Action callback)
    {
        _isTweening = true;
        _maxHP = max;
        if (max != 0)
        {
            Bar.fillAmount = (float)from / (float)max;
            _tweener = Bar.DOFillAmount((float)to / (float)max, 0.5f).OnComplete(() =>
            {
                if (callback != null)
                {
                    callback();
                }
            });
            _tweener.SetUpdate(true);
        }
        else
        {
            Bar.fillAmount = 0;
        }
    }

    public void MinusValueTween(int minus, int max, Action callback)
    {
        _isTweening = true;
        _maxHP = max;
        int current = Mathf.RoundToInt(max * Bar.fillAmount);
        current -= minus;
        if (max != 0)
        {
            _tweener = Bar.DOFillAmount((float)current / (float)max, 0.5f).OnComplete(() =>
            {
                if (callback != null)
                {
                    callback();
                }
            });
            _tweener.SetUpdate(true);
        }
        else
        {
            Bar.fillAmount = 0;
        }
    }

    protected virtual void UpdateData() 
    {
        if (_isTweening)
        {
            Label.text = Mathf.RoundToInt(_maxHP * Bar.fillAmount).ToString() + "/" + _maxHP.ToString();
        }
    }

    void Update()
    {
        UpdateData();
    }
}
