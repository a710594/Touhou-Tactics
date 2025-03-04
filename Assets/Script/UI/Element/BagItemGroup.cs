using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItemGroup : MonoBehaviour
{
    public Action<object> ScrollHandler;

    public ScrollView ScrollView;
    public Text NameLabel;
    public Text CommentLabel;
    public ButtonPlusGroup ButtonGroup;

    public void SetScrollView(ItemModel.CategoryEnum category)
    {
        if (category == ItemModel.CategoryEnum.Consumables)
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.Info.ConsumablesDic.Values));
        }
        else if(category == ItemModel.CategoryEnum.Food) 
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.Info.FoodList));
        }
        else if (category == ItemModel.CategoryEnum.Item)
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.Info.ItemDic.Values));
        }

        ButtonGroup.Clear();
        for (int i = 0; i < ScrollView.GridList.Count; i++)
        {
            for (int j=0; j<ScrollView.GridList[i].ScrollItemList.Count; j++) 
            {
                ButtonGroup.Add(ScrollView.GridList[i].ScrollItemList[j].GetComponent<ButtonPlusSingle>());
            }
        }
        ButtonGroup.CancelAllSelect();
    }

    public void SetName(string text) 
    {
        NameLabel.text = text;
    }

    public void SetComment(string text) 
    {
        CommentLabel.text = text;
    }

    private void ScrollItemOnClick(PointerEventData eventData, ButtonPlus button)
    {
        object data = button.Data;
        if (data is Item)
        {
            Item item = (Item)data;
            SetName(item.Data.Name);
            SetComment(item.Data.Comment);
        }
        else if(data is Consumables) 
        {
            Consumables consumables = (Consumables)data;
            SetName(consumables.Name);
            SetComment(consumables.Comment);
        }
        else if(data is Food) 
        {
            Food food = (Food)data;
            SetName(food.Name);
            SetComment(food.Comment);
        }

        ScrollHandler(data);
    }

    private void Awake()
    {
        ScrollView.ClickHandler += ScrollItemOnClick;
    }
}
