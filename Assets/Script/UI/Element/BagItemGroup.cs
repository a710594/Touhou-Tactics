using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            ScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.ConsumablesDic.Values));
        }
        else if(category == ItemModel.CategoryEnum.Food) 
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.FoodList));
        }
        else if (category == ItemModel.CategoryEnum.Card)
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.CardDic.Values));
        }
        else if (category == ItemModel.CategoryEnum.Item)
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.ItemDic.Values));
        }
    }

    public void SetComment(string text) 
    {
        CommentLabel.text = text;
    }

    private void ScrollItemOnClick(ScrollItem scrollItem)
    {
        object obj = scrollItem.Data;
        if (obj is Item)
        {
            Item item = (Item)obj;
            SetComment(item.Data.Comment);
        }
        else if(obj is Consumables) 
        {
            Consumables consumables = (Consumables)obj;
            SetComment(consumables.ItemData.Comment);
        }
        else if (obj is Card)
        {
            Card card = (Card)obj;
            SetComment(card.ItemData.Comment);
        }
        else if(obj is Food) 
        {
            Food food = (Food)obj;
            SetComment(food.Comment);
        }

        ScrollHandler(obj);
    }

    private void Awake()
    {
        ScrollView.ClickHandler += ScrollItemOnClick;
    }
}