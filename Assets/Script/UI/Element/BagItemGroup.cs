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
    public Text CommentLabel;

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
            SetComment(item.Data.Comment);
        }
        else if(data is Consumables) 
        {
            Consumables consumables = (Consumables)data;
            SetComment(consumables.Comment);
        }
        else if(data is Food) 
        {
            Food food = (Food)data;
            SetComment(food.Comment);
        }

        ScrollHandler(data);
    }

    private void Awake()
    {
        ScrollView.ClickHandler += ScrollItemOnClick;
    }
}
