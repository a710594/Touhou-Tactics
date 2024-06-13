using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollGrid : MonoBehaviour
{
    public Action<ScrollItem> ClickHandler;
    public Action<ScrollItem> DownHandler;
    public Action<ScrollItem> PressHandler;
    public Action<ScrollItem> UpHandler;
    public Action<ScrollItem> EnterHandler;
    public Action<ScrollItem> ExitHandler;

    public Image Background;
    public GridLayoutGroup Grid;


    [NonReorderable]
    public int ItemAmount;

    private List<ScrollItem> _scrollItemList = new List<ScrollItem>();

    public void Init(ScrollItem scrollItem, ScrollView.TypeEnum type, float length, float cellSizeX, float cellSizeY, float spacingX, float spacingY, int itemAmount)
    {
        Grid.cellSize = new Vector2(cellSizeX, cellSizeY);
        Grid.spacing = new Vector2(spacingX, spacingY);

        if (type == ScrollView.TypeEnum.Horizontal) 
        {
            ItemAmount = itemAmount;
            Background.rectTransform.sizeDelta = new Vector2(scrollItem.Background.rectTransform.sizeDelta.x, length);
        }
        else if(type == ScrollView.TypeEnum.Vertical)
        {
            ItemAmount = itemAmount;
            Background.rectTransform.sizeDelta = new Vector2(length, scrollItem.Background.rectTransform.sizeDelta.y);
        }

        ScrollItem item;
        for (int i = 0; i < ItemAmount; i++)
        {
            item = Instantiate(scrollItem);
            item.transform.SetParent(transform);
            item.ClickHandler = OnClick;
            item.DownHandler = OnDown;
            item.PressHandler = OnPress;
            item.UpHandler = OnUp;
            item.EnterHandler = OnEnter;
            item.ExitHandler = OnExit;
            _scrollItemList.Add(item);
        }
    }

    public void RefreshData(int index, List<object> list)
    {
        if(index >= 0)
        {
            for (int i = 0; i < _scrollItemList.Count; i++)
            {
                if (index * ItemAmount + i < list.Count)
                {
                    gameObject.SetActive(true);
                    _scrollItemList[i].gameObject.SetActive(true);
                    _scrollItemList[i].SetData(list[index * ItemAmount + i]);
                }
                else
                {
                    _scrollItemList[i].gameObject.SetActive(false);
                    if (i == 0) 
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void SetSelect(ScrollItem scrollItem)
    {
        for (int i = 0; i < _scrollItemList.Count; i++)
        {
            if (_scrollItemList[i].Equals(scrollItem))
            {
                _scrollItemList[i].SetSelected(true);
            }
            else
            {
                _scrollItemList[i].SetSelected(false);
            }
        }
    }

    private void OnClick(ScrollItem scrollItem)
    {
        if (ClickHandler != null)
        {
            ClickHandler(scrollItem);
        }
    }

    private void OnDown(ScrollItem scrollItem)
    {
        if (DownHandler != null)
        {
            DownHandler(scrollItem);
        }
    }

    private void OnPress(ScrollItem scrollItem)
    {
        if (PressHandler != null)
        {
            PressHandler(scrollItem);
        }
    }

    private void OnUp(ScrollItem scrollItem)
    {
        if (UpHandler != null)
        {
            UpHandler(scrollItem);
        }
    }

    private void OnEnter(ScrollItem scrollItem)
    {
        if (EnterHandler != null)
        {
            EnterHandler(scrollItem);
        }
    }

    private void OnExit(ScrollItem scrollItem)
    {
        if (ExitHandler != null)
        {
            ExitHandler(scrollItem);
        }
    }
}
