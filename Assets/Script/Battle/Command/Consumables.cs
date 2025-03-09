using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Consumables : Command
{
    public int Amount;
    public int Price;
    public ItemModel.CategoryEnum Category;

    public Consumables() { }

    public Consumables(int id, int amount)
    {
        ItemModel itemData = DataTable.Instance.ItemDic[id];
        ConsumablesModel consumablesData = DataTable.Instance.ConsumablesDic[id];
        ID = id;
        Amount = amount;
        Price = itemData.Price;
        Category = itemData.Category;
        Name = itemData.Name;
        Comment = itemData.Comment;
        if (consumablesData.EffectID != -1)
        {
            Effect = EffectFactory.GetEffect(consumablesData.EffectID);
        }

        Hit = consumablesData.Hit;
        Range = consumablesData.Range;
        RangeTarget = consumablesData.RangeTarget;
        AreaTarget = consumablesData.AreaTarget;
        AreaType = consumablesData.AreaType;
        Track = consumablesData.Track;
        ArrayList = Utility.GetAreaList(consumablesData.Area);
    }

    public void Init()
    {
        ConsumablesModel consumablesData = DataTable.Instance.ConsumablesDic[ID];
        if (consumablesData.EffectID != -1)
        {
            Effect = EffectFactory.GetEffect(consumablesData.EffectID);
        }
    }
}
