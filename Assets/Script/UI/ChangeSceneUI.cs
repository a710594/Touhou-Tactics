using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChangeSceneUI : MonoBehaviour
{
    public enum TypeEnum 
    {
        None,
        Loading,
        Fade,
    }

    public LoadingUI LoadingUI;
    public FadeUI FadeUI;


    public void Open(TypeEnum type, Action callback) 
    {
        if(type == TypeEnum.Loading) 
        {
            LoadingUI.Open(callback);
        }
        else if(type == TypeEnum.Fade) 
        {
            FadeUI.Open(callback);
        }
    }

    public void Close(TypeEnum type)
    {
        if (type == TypeEnum.Loading)
        {
            LoadingUI.Close();
        }
        else if (type == TypeEnum.Fade)
        {
            FadeUI.Close();
        }
    }
}
