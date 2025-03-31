using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemGroup : MonoBehaviour
{
    public Action<ShopModel> ShopDataHandler;
    public Action<object> SellHandler;

    public ScrollView ScrollView;
    public Text CommentLabel;
    public Text MaterialLabel;
    public ButtonPlusGroup ButtonGroup;

    private ButtonPlus _selectedButton;

    public void Init()
    {
        _selectedButton = null;
        ButtonGroup.CancelAllSelect();
    }

    public void SetScrollViewBuy(ItemModel.CategoryEnum category)
    {
        ScrollView.SetData(new List<object>(DataTable.Instance.ShopItemDic[category]));
        SetButtonGroup();
        MaterialLabel.text = "";
    }

    public void SetScrollViewSell(ItemModel.CategoryEnum category)
    {
        SetButtonGroup();
        if (category == ItemModel.CategoryEnum.Consumables) 
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.Info.ConsumablesDic.Values));
        }
        else if (category == ItemModel.CategoryEnum.Equip)
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.Info.EquipList));
        }
        else if (category == ItemModel.CategoryEnum.Food)
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.Info.FoodList));
        }
        else if (category == ItemModel.CategoryEnum.Material)
        {
            ScrollView.SetData(new List<object>(ItemManager.Instance.Info.ItemDic.Values));
        }

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
            itemData = DataTable.Instance.ItemDic[shopData.MaterialIDList[i]];
            MaterialLabel.text += itemData.Name + " " + ItemManager.Instance.GetAmount(itemData.ID) + "/" + shopData.MaterialAmountList[i] + " ";
        }
    }

    private void SetButtonGroup()
    {
        ButtonGroup.Clear();
        for (int i = 0; i < ScrollView.GridList.Count; i++)
        {
            for (int j = 0; j < ScrollView.GridList[i].ScrollItemList.Count; j++)
            {
                ButtonGroup.Add(ScrollView.GridList[i].ScrollItemList[j].GetComponent<ButtonPlusSingle>());
            }
        }
        ButtonGroup.CancelAllSelect();
        if (_selectedButton != null) 
        {
            object data = _selectedButton.Data;
            if (data is ShopModel) //buy
            {
                ButtonGroup.SetSelect(_selectedButton);
            }
            else //sell
            {
                if (data is Item)
                {
                    Item item = (Item)data;
                    if(ItemManager.Instance.GetAmount(item.ID) > 0) 
                    {
                        ButtonGroup.SetSelect(_selectedButton);
                    }
                    else
                    {
                        SetComment("");
                    }
                }

                MaterialLabel.text = "";
                SellHandler(data);
            }
        }
    }

    private void ScrollItemOnClick(PointerEventData eventData, ButtonPlus button)
    {
        _selectedButton = button;
        object data = button.Data;
        if (data is ShopModel) //buy
        {
            ShopModel shopData = (ShopModel)data;
            SetComment(DataTable.Instance.ItemDic[shopData.ID].Comment);
            SetMaterial(shopData);
            ShopDataHandler(shopData);
        }
        else //sell
        {
            if (data is Item)
            {
                Item item = (Item)data;
                SetComment(item.Data.Comment);
            }
            else if(data is Food) 
            {
                Food food = (Food)data;
                SetComment(food.Comment);
            }
            MaterialLabel.text = "";
            SellHandler(data);
        }
    }

    private void Awake()
    {
        ScrollView.ClickHandler += ScrollItemOnClick;
    }
}
