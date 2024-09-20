using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPlus : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<PointerEventData, ButtonPlus> ClickHandler;
    public Action<ButtonPlus> PressHandler;
    public Action<ButtonPlus> DownHandler;
    public Action<ButtonPlus> UpHandler;
    public Action<ButtonPlus> EnterHandler;
    public Action<ButtonPlus> ExitHandler;
    public Action<ButtonPlus> DragBegingHandler;
    public Action<PointerEventData, ButtonPlus> DragHandler;
    public Action<ButtonPlus> DragEndHandler;

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

    public virtual void SetData(object data)
    {
        Data = data;
    }

    public void SetColor(Color color)
    {
        Image.color = color;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (DragBegingHandler != null)
        {
            DragBegingHandler(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DragHandler != null) 
        {
            DragHandler(eventData, this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (DragEndHandler != null) 
        {
            DragEndHandler(this);
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!_longPressTriggered || DownThreshold == 0)
        {
            if (ClickHandler != null)
            {
                ClickHandler(eventData, this);
            }
        }
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
            else if (Time.time - _startPressTime > PressDuration)
            {
                _startPressTime = Time.time;
                if (PressHandler != null)
                {
                    PressHandler(this);
                }
            }
        }
    }
}
