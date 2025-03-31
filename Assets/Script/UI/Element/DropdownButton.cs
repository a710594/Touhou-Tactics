using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownButton : DropdownNode
{
    private static readonly Color _darkGray = new Color(0.5f, 0.5f, 0.5f);
    private static readonly Color _lightGray = new Color(0.7f, 0.7f, 0.7f);

    public ButtonPlus Button;
    public DropdownGroup DropdownGroup;
    public Image Arrow;

    [NonSerialized]
    public object Data = null;

    private bool _hasUse = false;
    private DropdownRoot _root;

    public void SetData(string text, object data, DropdownRoot root) 
    {
        Button.Label.text = text;
        Data = data;
        _root = root;

        if (data is List<KeyValuePair<string, object>>)
        {
            Arrow.gameObject.SetActive(true);
        }
        else
        {
            Arrow.gameObject.SetActive(false);
        }
    }

    public void SetHasUse(bool hasUse) 
    {
        _hasUse = hasUse;
        if (!hasUse) 
        {
            Button.Image.color = Color.white;
            Button.Label.color = Color.black;
        }
        else
        {
            Button.Image.color = _darkGray;
            Button.Label.color = Color.white;
        }
    }

    private void OnEnter(ButtonPlus button)
    {
        if (!_hasUse) 
        {
            Button.Image.color = _lightGray;
            Button.Label.color = Color.black;
        }
        else
        {
            Button.Image.color = Color.black;
            Button.Label.color = Color.white;
        }

        if (_root != null)
        {
            _root.ButtonOnEnter(Data, this, DropdownGroup);
        }
    }

    private void OnExit(ButtonPlus button)
    {
        if (!_hasUse)
        {
            Button.Image.color = Color.white;
            Button.Label.color = Color.black;
        }
        else
        {
            Button.Image.color = _darkGray;
            Button.Label.color = Color.white;
        }

        if (_root != null)
        {
            _root.ButtonOnExit();
        }
    }

    private void OnClick(PointerEventData eventData, ButtonPlus button) 
    {
        if (_root != null)
        {
            _root.ButtonOnClick(Data);
        }
    }

    private void Awake()
    {
        Button.EnterHandler += OnEnter;
        Button.ExitHandler += OnExit;
        Button.ClickHandler += OnClick;
    }
}
