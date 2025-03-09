using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagInfo
{
    public int Money;
    public int Key;
    public Dictionary<int, Item> ItemDic = new Dictionary<int, Item>();
    public Dictionary<int, Consumables> ConsumablesDic = new Dictionary<int, Consumables>();
    public List<Food> FoodList = new List<Food>();
    public List<Equip> EquipList = new List<Equip>();

    public BagInfo()
    {
        Money = 100;
        Key = 0;
    }

    public List<Item> GetFoodMaterial() 
    {
        List<Item> list = new List<Item>();
        foreach(KeyValuePair<int, Item> pair in ItemDic) 
        {
            if (DataTable.Instance.FoodMaterialDic.ContainsKey(pair.Key)) 
            {
                list.Add(pair.Value);
            }
        }
        return list;
    }
}
