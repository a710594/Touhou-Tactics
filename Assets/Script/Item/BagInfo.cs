using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagInfo
{
    public int Money;
    public List<Item> ItemList = new List<Item>();

    public BagInfo()
    {
        Money = 100;
    }

    public void Save(Dictionary<ItemModel.CategoryEnum, Dictionary<int, Item>> itemDic)
    {
        ItemList.Clear();
        foreach (KeyValuePair<ItemModel.CategoryEnum, Dictionary<int, Item>> pair1 in itemDic)
        {
            foreach (KeyValuePair<int, Item> pair2 in pair1.Value)
            {
                ItemList.Add(pair2.Value);
            }
        }
    }
}
