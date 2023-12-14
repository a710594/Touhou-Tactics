using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemGroup : MonoBehaviour
{
    public Action<ShopModel> ShopDataHandler;
    public Action<object> SellHandler;

    public ScrollView ScrollView;
    public Text CommentLabel;
    public Text MaterialLabel;

    public void SetScrollViewBuy(ItemModel.CategoryEnum category)
    {
        ScrollView.SetData(new List<object>(DataContext.Instance.ShopItemDic[category]));
    }

    public void SetScrollViewSell(ItemModel.CategoryEnum category)
    {
        if (category == ItemModel.CategoryEnum.Card)
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.CardDic.Values));
        }
        else if(category == ItemModel.CategoryEnum.Consumables) 
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.ConsumablesDic.Values));
        }
        else if (category == ItemModel.CategoryEnum.Equip)
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.EquipList));
        }
        else if (category == ItemModel.CategoryEnum.Food)
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.FoodList));
        }
        else if (category == ItemModel.CategoryEnum.Item)
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.ItemDic.Values));
        }
    }

    public void CancelScrollViewSelect() 
    {
        ScrollView.CancelSelect();
        MaterialLabel.text = "";
    }

    public void SetComment(string text)
    {
        CommentLabel.text = text;
    }

    public void SetMaterial(ShopModel shopData)
    {
        ItemModel itemData;
        MaterialLabel.text = "";
        for (int i = 0; i < shopData.MaterialIDList.Count; i++)
        {
            itemData = DataContext.Instance.ItemDic[shopData.MaterialIDList[i]];
            MaterialLabel.text += itemData.Name + " " + ItemManager.Instance.GetAmount(itemData.Category, itemData.ID) + "/" + shopData.MaterialAmountList[i] + " ";
        }
    }

    private void ScrollItemOnClick(ScrollItem scrollItem)
    {
        object obj = scrollItem.Data;
        if (obj is ShopModel) //buy
        {
            ShopModel data = (ShopModel)obj;
            SetComment(DataContext.Instance.ItemDic[data.ID].Comment);
            SetMaterial(data);
            ShopDataHandler(data);
        }
        else //sell
        {
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
            else if(obj is Card) 
            {
                Card card = (Card)obj;
                SetComment(card.ItemData.Comment);
            }
            else if(obj is Food) 
            {
                Food food = (Food)obj;
                SetComment(food.Comment);
            }
            MaterialLabel.text = "";
            SellHandler(obj);
        }
    }

    private void Awake()
    {
        ScrollView.ClickHandler += ScrollItemOnClick;
    }
}
