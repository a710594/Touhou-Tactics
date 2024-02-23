using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Card
{
    public int ID;
    public int Amount;
    [NonSerialized]
    public ItemModel ItemData;
    [NonSerialized]
    public CardModel CardData;
    [NonSerialized]
    public Effect Effect;

    public Card() { }

    public Card(int id, int amount)
    {
        ID = id;
        ItemData = DataContext.Instance.ItemDic[id];
        CardData = DataContext.Instance.CardDic[id];
        Amount = amount;
    }

    public void Init()
    {
        ItemData = DataContext.Instance.ItemDic[ID];
        CardData = DataContext.Instance.CardDic[ID];
        if (CardData.EffectID != -1)
        {
            Effect = EffectFactory.GetEffect(CardData.EffectID);
        }
    }
}
