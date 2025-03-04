using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollGrid : MonoBehaviour
{
    public Action<PointerEventData, ButtonPlus> ClickHandler;
    public Action<ButtonPlus> DownHandler;
    public Action<ButtonPlus> PressHandler;
    public Action<ButtonPlus> UpHandler;
    public Action<ButtonPlus> EnterHandler;
    public Action<ButtonPlus> ExitHandler;
    public Action<ButtonPlus> DragBegingHandler;
    public Action<PointerEventData, ButtonPlus> DragHandler;
    public Action<ButtonPlus> DragEndHandler;

    public Image Background;
    public GridLayoutGroup Grid;


    [NonSerialized]
    public int ItemAmount;
    [NonSerialized]
    public List<ScrollItem> ScrollItemList = new List<ScrollItem>();

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
            item.DragBegingHandler = OnBeginDrag;
            item.DragHandler = OnDrag;
            item.DragEndHandler = OnEndDrag;
            ScrollItemList.Add(item);
        }
    }

    public void RefreshData(int index, List<object> list)
    {
        if(index >= 0)
        {
            for (int i = 0; i < ScrollItemList.Count; i++)
            {
                if (index * ItemAmount + i < list.Count)
                {
                    gameObject.SetActive(true);
                    ScrollItemList[i].gameObject.SetActive(true);
                    ScrollItemList[i].SetData(list[index * ItemAmount + i]);
                }
                else
                {
                    ScrollItemList[i].gameObject.SetActive(false);
                    if (i == 0) 
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void OnClick(PointerEventData eventData, ButtonPlus buttonPlus)
    {
        if (ClickHandler != null)
        {
            ClickHandler(eventData, buttonPlus);
        }
    }

    private void OnDown(ButtonPlus buttonPlus)
    {
        if (DownHandler != null)
        {
            DownHandler(buttonPlus);
        }
    }

    private void OnPress(ButtonPlus buttonPlus)
    {
        if (PressHandler != null)
        {
            PressHandler(buttonPlus);
        }
    }

    private void OnUp(ButtonPlus buttonPlus)
    {
        if (UpHandler != null)
        {
            UpHandler(buttonPlus);
        }
    }

    private void OnEnter(ButtonPlus buttonPlus)
    {
        if (EnterHandler != null)
        {
            EnterHandler(buttonPlus);
        }
    }

    private void OnExit(ButtonPlus buttonPlus)
    {
        if (ExitHandler != null)
        {
            ExitHandler(buttonPlus);
        }
    }

    private void OnBeginDrag(ButtonPlus buttonPlus)
    {
        if (DragBegingHandler != null)
        {
            DragBegingHandler(buttonPlus);
        }
    }

    private void OnDrag(PointerEventData eventData, ButtonPlus buttonPlus)
    {
        if (DragHandler != null)
        {
            DragHandler(eventData, buttonPlus);
        }
    }

    private void OnEndDrag(ButtonPlus buttonPlus)
    {
        if (DragEndHandler != null)
        {
            DragEndHandler(buttonPlus);
        }
    }
}
