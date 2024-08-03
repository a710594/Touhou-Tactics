using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{

    public float WaitTime;
    public Text TextLabel;

    public bool IsTyping
    {
        get
        {
            if (_charIndex == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private int _charIndex = -1;
    private string _dialog;
    private string _tempText;
    private Timer _timer = new Timer();

    public void Show(string dialog)
    {
        if (_charIndex == -1)
        {
            _charIndex = 0;
            _dialog = dialog;

            if (WaitTime == 0)
            {
                SetText();
            }
            else
            {
                //StartCoroutine(Timer(WaitTime, NextChar));
                _timer.Start(WaitTime, NextChar, true);
            }
        }
    }

    public void SetText()
    {
        _charIndex = -1;
        TextLabel.text = _dialog;
    }

    public void ClearText()
    {
        TextLabel.text = string.Empty;
    }

    private void NextChar()
    {
        if (_charIndex != -1 && _charIndex <= _dialog.Length)
        {
            _tempText = _dialog.Insert(_charIndex, "<color=#00000000>");
            _tempText = _tempText.Insert(_tempText.Length, "</color>");
            TextLabel.text = _tempText;

            _charIndex++;
            //StartCoroutine(Timer(WaitTime, NextChar));
        }
        else
        {
            _charIndex = -1;
            _timer.StopLoop();
        }
    }

    private void OnDestroy()
    {
        _timer.Stop();
    }

    private IEnumerator Timer(float time, Action callback)
    {
        yield return null;

        if (callback != null)
        {
            callback();
        }
    }
}