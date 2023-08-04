using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public ItemModel.CategoryEnum Category;
    public int ID;
    public int Amount;
    [NonSerialized]
    public ItemModel Data;
    [NonSerialized]
    public Effect Effect;

    public Item() { }

    public Item(ItemModel.CategoryEnum category, int id, int amount)
    {
        Category = category;
        ID = id;
        Data = DataContext.Instance.ItemDic[category][id];
        Amount = amount;
        Effect = EffectFactory.GetEffect(Data.EffectType, Data.EffectID);
    }

    public virtual void SetEffect(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    {
        Effect.SetEffect(user, target, floatingList, characterList);
        user.HasUseItem = true;
        user.ActionCount--;
        if (user.CurrentPP < BattleCharacterInfo.MaxPP)
        {
            user.CurrentPP++;
        }
        ItemManager.Instance.MinusItem(this, 1);
    }
}
