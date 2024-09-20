using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollItem : ButtonPlus
{
    public Image Background;
    public Image SelectedImage;


    public void SetSelected(bool show)
    {
        if (SelectedImage != null)
        {
            SelectedImage.gameObject.SetActive(show);
        }
    }

    protected virtual void Awake()
    {
        if (SelectedImage != null)
        {
            SelectedImage.gameObject.SetActive(false);
        }
    }
}