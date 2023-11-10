using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollGrid : MonoBehaviour
{
    public Action<object, ScrollItem> ItemOnClickHandler;

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
            item.OnClickHandler += ItemOnClick;
            _scrollItemList.Add(item);
        }
    }

    public void RefreshData(int index, List<object> list)
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

    private void ItemOnClick(object obj, ScrollItem item)
    {
        if (ItemOnClickHandler != null)
        {
            ItemOnClickHandler(obj, item);
        }
    }
}
