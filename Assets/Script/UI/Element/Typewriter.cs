using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    public enum TextType
    {
        Text,
        TextMesh,
    }

    public TextType Type;
    [SerializeField]
    public float WaitTime;
    public Text TextLabel;
    public TextMesh MeshLabel;

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
                StartCoroutine(Timer(WaitTime, NextChar));
            }
        }
    }

    public void SetText()
    {
        _charIndex = -1;
        if (Type == TextType.Text)
        {
            TextLabel.text = _dialog;
        }
        else
        {
            MeshLabel.text = _dialog;
        }
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
            if (Type == TextType.Text)
            {
                TextLabel.text = _tempText;
            }
            else
            {
                MeshLabel.text = _tempText;
            }

            _charIndex++;
            StartCoroutine(Timer(WaitTime, NextChar));
        }
        else
        {
            _charIndex = -1;
        }
    }

    private IEnumerator Timer(float time, Action callback)
    {
        yield return new WaitForSeconds(time);

        if (callback != null)
        {
            callback();
        }
    }
}