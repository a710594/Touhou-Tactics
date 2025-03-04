using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPlusSingle : MonoBehaviour
{
    public enum TypeEnum
    {
        GameObject,
        Color,
        Sprite,
    }

    public Action<ButtonPlus> ClickHandler;
    public Action<ButtonPlus> EnterHandler;
    public Action<ButtonPlus> ExitHandler;

    public TypeEnum Type = TypeEnum.GameObject;
    public GameObject Select;
    public GameObject NotSelect;
    public Color SelectColor;
    public Color NotSelectColor;
    public Sprite SelectImage;
    public Sprite NotSelectImage;
    public ButtonPlus Button;

    public ButtonPlusSingle SelectOnUp;
    public ButtonPlusSingle SelectOnDown;
    public ButtonPlusSingle SelectOnLeft;
    public ButtonPlusSingle SelectOnRight;

    public void SetSelect(bool isSelected)
    {
        if (Type == TypeEnum.GameObject)
        {
            if (isSelected) 
            {              
                if (Select != null)
                {
                    Select.SetActive(true);
                }
                if (NotSelect != null)
                {
                    NotSelect.SetActive(false);
                }
            }
            else 
            {
                if (Select != null)
                {
                    Select.SetActive(false);
                }
                if (NotSelect != null)
                {
                    NotSelect.SetActive(true);
                }
            }
        }
        else if (Type == TypeEnum.Color)
        {
            if (isSelected)
            {
                Button.Image.color = SelectColor;
            }
            else
            {
                Button.Image.color = NotSelectColor;
            }
        }
        else if (Type == TypeEnum.Sprite)
        {
            if (isSelected)
            {
                Button.Image.overrideSprite = SelectImage;
            }
            else
            {
                Button.Image.overrideSprite = NotSelectImage;
            }
        }
    }

    private void OnClick(PointerEventData eventData, ButtonPlus button)
    {
        if (ClickHandler != null)
        {
            ClickHandler(button);
        }
    }

    private void OnEnter(ButtonPlus button)
    {
        SetSelect(true);

        if (EnterHandler != null) 
        {
            EnterHandler(button);
        }
    }

    private void OnExit(ButtonPlus button)
    {
        SetSelect(false);

        if (ExitHandler != null)
        {
            ExitHandler(button);
        }
    }

    private void Awake()
    {
        if (Select != null)
        {
            Select.SetActive(false);
        }
        Button.ClickHandler += OnClick;
        Button.EnterHandler += OnEnter;
        Button.ExitHandler += OnExit;
    }
}
