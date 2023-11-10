using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagInfo
{
    public int Money;
    public Dictionary<int, Item> ItemDic = new Dictionary<int, Item>();
    public Dictionary<int, Consumables> ConsumablesDic = new Dictionary<int, Consumables>();
    public Dictionary<int, Card> CardDic = new Dictionary<int, Card>();
    public List<Food> FoodList = new List<Food>();
    public List<Equip> EquipList = new List<Equip>();

    public BagInfo()
    {
        Money = 100;
    }
}
