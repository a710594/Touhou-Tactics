using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollItem : MonoBehaviour
{
    public Action<object, ScrollItem> OnClickHandler;
    public Action<object> OnDownHandler;
    public Action<object> OnPressHandler;
    public Action<object> OnUpHandler;

    //public Text Label;
    public Image Background;
    public Image SelectedImage;
    public ButtonPlus Button;

    protected object _data;

    public virtual void SetData(object obj)
    {
        _data = obj;
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
        if (OnClickHandler != null)
        {
            OnClickHandler(_data, this);
        }
    }

    private void OnDown(object data)
    {
        if (OnDownHandler != null)
        {
            OnDownHandler(_data);
        }
    }

    private void OnPress(object data)
    {
        if (OnPressHandler != null)
        {
            OnPressHandler(_data);
        }
    }

    private void OnUp(object data)
    {
        if (OnUpHandler != null)
        {
            OnUpHandler(_data);
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
        }
    }
}