using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Consumables 
{
    public int ID;
    public int Amount;
    [NonSerialized]
    public ItemModel ItemData;
    [NonSerialized]
    public ConsumablesModel ConsumablesData;
    [NonSerialized]
    public Effect Effect;

    public Consumables() { }

    public Consumables(int id, int amount)
    {
        ID = id;
        ItemData = DataContext.Instance.ItemDic[id];
        ConsumablesData = DataContext.Instance.ConsumablesDic[id];
        Amount = amount;
        if (ConsumablesData.EffectID != -1)
        {
            Effect = EffectFactory.GetEffect(ConsumablesData.EffectID);
        }
    }

    public void Init()
    {
        ItemData = DataContext.Instance.ItemDic[ID];
        ConsumablesData = DataContext.Instance.ConsumablesDic[ID];
        if (ConsumablesData.EffectID != -1)
        {
            Effect = EffectFactory.GetEffect(ConsumablesData.EffectID);
        }
    }
}
