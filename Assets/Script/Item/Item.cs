using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Data = DataTable.Instance.ItemDic[id];
        Amount = amount;
    }

    public void Init()
    {
        Data = DataTable.Instance.ItemDic[ID];
    }
}
