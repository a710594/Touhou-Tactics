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
        ItemModel itemData = DataContext.Instance.ItemDic[id];
        ConsumablesModel consumablesData = DataContext.Instance.ConsumablesDic[id];
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
        CastTarget = consumablesData.CastTarget;
        EffectTarget = consumablesData.EffectTarget;
        Track = consumablesData.Track;
        AreaList = Utility.GetAreaList(consumablesData.Area);
    }

    public void Init()
    {
        ConsumablesModel consumablesData = DataContext.Instance.ConsumablesDic[ID];
        if (consumablesData.EffectID != -1)
        {
            Effect = EffectFactory.GetEffect(consumablesData.EffectID);
        }
    }
}
