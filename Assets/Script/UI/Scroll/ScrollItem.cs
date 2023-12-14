using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollItem : MonoBehaviour
{
    public Action<ScrollItem> ClickHandler;
    public Action<ScrollItem> DownHandler;
    public Action<ScrollItem> PressHandler;
    public Action<ScrollItem> UpHandler;
    public Action<ScrollItem> EnterHandler;
    public Action<ScrollItem> ExitHandler;

    //public Text Label;
    public Image Background;
    public Image SelectedImage;
    public ButtonPlus Button;

    public object Data;

    public virtual void SetData(object obj)
    {
        Data = obj;
        //Button.Label.text = (string)obj;
    }

    public void SetSelected(bool show)
    {
        if (SelectedImage != null)
        {
            SelectedImage.gameObject.SetActive(show);
        }
    }

    private void OnClick(ButtonPlus button)
    {
        if (ClickHandler != null)
        {
            ClickHandler(this);
        }
    }

    private void OnDown(object data)
    {
        if (DownHandler != null)
        {
            DownHandler(this);
        }
    }

    private void OnPress(object data)
    {
        if (PressHandler != null)
        {
            PressHandler(this);
        }
    }

    private void OnUp(object data)
    {
        if (UpHandler != null)
        {
            UpHandler(this);
        }
    }

    private void OnEnter(object data)
    {
        if (EnterHandler != null)
        {
            EnterHandler(this);
        }
    }

    private void OnExit(object data)
    {
        if (ExitHandler != null)
        {
            ExitHandler(this);
        }
    }

    protected virtual void Awake()
    {
        if (SelectedImage != null)
        {
            SelectedImage.gameObject.SetActive(false);
        }

        if (Button != null)
        {
            Button.ClickHandler = OnClick;
            Button.DownHandler = OnDown;
            Button.PressHandler = OnPress;
            Button.UpHandler = OnUp;
            Button.EnterHandler = OnEnter;
            Button.ExitHandler = OnExit;
        }
    }
}