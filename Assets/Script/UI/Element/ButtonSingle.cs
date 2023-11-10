using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSingle : MonoBehaviour
{
    public enum TypeEnum
    {
        GameObject,
        Color,
        Sprite,
    }

    public Action<GameObject> ClickHandler;

    public TypeEnum Type = TypeEnum.GameObject;
    public GameObject Select;
    public Color SelectColor;
    public Color NotSelectColor;
    public Sprite SelectImage;
    public Sprite NotSelectImage;
    public Button Button;

    public void SetSelected(bool isSelected)
    {
        if (Type == TypeEnum.GameObject)
        {
            Select.SetActive(isSelected);
        }
        else if(Type == TypeEnum.Color)
        {
            if (isSelected)
            {
                Button.image.color = SelectColor;
            }
            else
            {
                Button.image.color = NotSelectColor;
            }
        }
        else if (Type == TypeEnum.Sprite)
        {
            if (isSelected)
            {
                Button.image.overrideSprite = SelectImage;
            }
            else
            {
                Button.image.overrideSprite = NotSelectImage;
            }
        }
    }

    void OnClick()
    {
        if (ClickHandler != null)
        {
            ClickHandler(gameObject);
        }
    }

    private void Awake()
    {
        if (Select != null)
        {
            Select.SetActive(false);
        }
        Button.onClick.AddListener(OnClick);
    }
}
