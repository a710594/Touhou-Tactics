using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScrollItem : ScrollItem
{
    public Text NameLabel;
    public Text AmountLabel;
    public Text PriceLabel;

    public override void SetData(object obj)
    {
        base.SetData(obj);

        if (obj is ShopModel)
        {
            ShopModel shopData = (ShopModel)obj;
            ItemModel itemData = DataContext.Instance.ItemDic[shopData.ID];
            NameLabel.text = itemData.Name;
            if(itemData.Category == ItemModel.CategoryEnum.Card || itemData.Category == ItemModel.CategoryEnum.Consumables || itemData.Category == ItemModel.CategoryEnum.Item) 
            {
                AmountLabel.text = ItemManager.Instance.GetAmount(itemData.Category, shopData.ID).ToString();
            }
            else 
            {
                AmountLabel.text = "";
            }
            PriceLabel.text = shopData.Price + "$";
        }
        else
        {
            if (obj is Item)
            {
                Item item = (Item)obj;
                NameLabel.text = item.Data.Name;
                AmountLabel.text = item.Amount.ToString();
                PriceLabel.text = item.Data.Price + "$";
            }
            else if(obj is Consumables) 
            {
                Consumables consumables = (Consumables)obj;
                NameLabel.text = consumables.ItemData.Name;
                AmountLabel.text = consumables.Amount.ToString();
                PriceLabel.text = consumables.ItemData.Price + "$";
            }
            else if(obj is Food) 
            {
                Food food = (Food)obj;
                NameLabel.text = food.Name;
                AmountLabel.text = "";
                PriceLabel.text = food.Price + "$";
            }
            else if(obj is Card) 
            {
                Card card = (Card)obj;
                NameLabel.text = card.ItemData.Name;
                AmountLabel.text = card.Amount.ToString();
                PriceLabel.text = card.ItemData.Price + "$";
            }
            else
            {
                Equip equip = (Equip)obj;
                NameLabel.text = equip.Name;
                AmountLabel.text = "";
                PriceLabel.text = equip.Price + "$";
            }
        }
    }
}