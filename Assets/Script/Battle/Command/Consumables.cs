using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Consumables : Command
{
    public int ID;
    public int Amount;
    public string Name;
    public string Comment;
    public int Price;

    public Consumables() { }

    public Consumables(int id, int amount)
    {
        ItemModel itemData = DataContext.Instance.ItemDic[id];
        ConsumablesModel consumablesData = DataContext.Instance.ConsumablesDic[id];
        ID = id;
        Amount = amount;
        Name = itemData.Name;
        Comment = itemData.Comment;
        if (consumablesData.EffectID != -1)
        {
            Effect = EffectFactory.GetEffect(consumablesData.EffectID);
        }
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
