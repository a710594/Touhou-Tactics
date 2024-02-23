﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPlus : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    public Action<ButtonPlus> ClickHandler;
    public Action<ButtonPlus> PressHandler;
    public Action<ButtonPlus> DownHandler;
    public Action<ButtonPlus> UpHandler;
    public Action<ButtonPlus> EnterHandler;
    public Action<ButtonPlus> ExitHandler;

    public float DownThreshold = 0.2f; //開始 Down 事件
    public float PressDuration = 0.1f; //Down 之後執行 Press 的週期
    public Button Button;
    public Text Label;
    public Image Image;
    public object Data = null;

    private bool _isPointerDown = false;
    private bool _longPressTriggered = false;
    private float _startDownTime;
    private float _startPressTime;

    public void SetData(object data)
    {
        Data = data;
    }

    public void SetColor(Color color)
    {
        Image.color = color;
    }

    private void Update()
    {
        if (_isPointerDown)
        {
            if (!_longPressTriggered)
            {
                if (Time.time - _startDownTime >= DownThreshold)
                {
                    _longPressTriggered = true;
                    _startPressTime = Time.time;
                    if (DownHandler != null)
                    {
                        DownHandler(this);
                    }
                }
            }
            else if(Time.time -_startPressTime > PressDuration)
            {
                _startPressTime = Time.time;
                if (PressHandler != null)
                {
                    PressHandler(this);
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _startDownTime = Time.time;
        _isPointerDown = true;
        _longPressTriggered = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;

        if (UpHandler != null)
        {
            UpHandler(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(EnterHandler!= null) 
        {
            EnterHandler(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerDown = false;

        if (ExitHandler != null) 
        {
            ExitHandler(this);
        }
    }

    public void OnClick()
    {
        if (!_longPressTriggered || DownThreshold == 0)
        {
            if (ClickHandler != null)
            {
                ClickHandler(this);
            }
        }
    }

    protected void Awake()
    {
        //if (Button != null)
        //{
        //    Button.onClick.AddListener(OnClick);
        //}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
}
