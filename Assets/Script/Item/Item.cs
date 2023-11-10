using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//道具中的素材類別
public class Item 
{
    public int ID;
    public int Amount;
    [NonSerialized]
    public ItemModel Data;

    public Item() { }

    public Item(int id, int amount)
    {
        ID = id;
        Data = DataContext.Instance.ItemDic[id];
        Amount = amount;
    }

    public void Init() 
    {
        Data = DataContext.Instance.ItemDic[ID];
        //if (Data.EffectID != -1)
        //{
        //    Effect = EffectFactory.GetEffect(Data.EffectID);
        //}
    }

    //public virtual void SetEffect(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    //{
    //    Effect.SetEffect(user, target, floatingList, characterList);
    //    user.HasUseItem = true;
    //    user.ActionCount--;
    //    if (user.CurrentPP < BattleCharacterInfo.MaxPP)
    //    {
    //        user.CurrentPP++;
    //    }
    //    ItemManager.Instance.MinusItem(this, 1);
    //}
}
