using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public virtual void UseEffect(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    {
        Effect.Use(user, target, floatingList, characterList);
        user.HasUseItem = true;
        user.ActionCount--;
        if (user.CurrentPP < BattleCharacterInfo.MaxPP)
        {
            user.CurrentPP++;
        }
        ItemManager.Instance.MinusItem(ID, 1);
    }

}
