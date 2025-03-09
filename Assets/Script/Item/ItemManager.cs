using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class ItemManager
{
    public static readonly int KeyID = 27;
    private readonly string _fileName = "BagInfo";
    private readonly int _maxEquipCount = 10;

    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemManager();
            }
            return _instance;
        }
    }

    public BagInfo Info;
    
    public void Init(BagInfo info) 
    {
        Info = info;
    }

    public void AddItem(int id, int amount)
    {
        ItemModel data = DataTable.Instance.ItemDic[id];

        if (data.Category == ItemModel.CategoryEnum.Item)
        {
            if (!Info.ItemDic.ContainsKey(id))
            {
                Item item = new Item(id, amount);
                Info.ItemDic.Add(id, item);
            }
            else
            {
                Info.ItemDic[id].Amount += amount;
            }
        }
        else if (data.Category == ItemModel.CategoryEnum.Consumables)
        {
            if (!Info.ConsumablesDic.ContainsKey(id))
            {
                Consumables consumables = new Consumables(id, amount);
                Info.ConsumablesDic.Add(id, consumables);
            }
            else
            {
                Info.ConsumablesDic[id].Amount += amount;
            }
        }
        else if(data.Category == ItemModel.CategoryEnum.Equip) 
        {
            AddEquip(id);
        }
    }

    public bool MinusItem(int id, int amount)
    {
        ItemModel data = DataTable.Instance.ItemDic[id];
        if (data.Category == ItemModel.CategoryEnum.Item)
        {
            Item item = Info.ItemDic[id];
            if (amount <= item.Amount)
            {
                item.Amount -= amount;
                if (item.Amount == 0)
                {
                    Info.ItemDic.Remove(item.Data.ID);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (data.Category == ItemModel.CategoryEnum.Consumables)
        {
            Consumables consumables = Info.ConsumablesDic[id];
            if (amount <= consumables.Amount)
            {
                consumables.Amount -= amount;
                if (consumables.Amount == 0)
                {
                    Info.ConsumablesDic.Remove(consumables.ID);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool AddEquip(int id) 
    {
        if (Info.EquipList.Count < _maxEquipCount)
        {
            Equip equip = new Equip(id);
            Info.EquipList.Add(equip);

            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddEquip(Equip equip) 
    {
        Info.EquipList.Add(equip);
    }

    public void MinusEquip(Equip equip)
    {
        Info.EquipList.Remove(equip);
    }

    public void AddFood(Food food) 
    {
        Info.FoodList.Add(food);
    }

    public void MinusFood(Food food)
    {
        Info.FoodList.Remove(food);
    }

    public List<object> GetBattleItemList()
    {
        List<object> resultList = new List<object>();

        foreach (KeyValuePair<int, Consumables> pair in Info.ConsumablesDic) 
        {
            resultList.Add(pair.Value);
        }

        for(int i=0; i<Info.FoodList.Count; i++)
        {
            resultList.Add(Info.FoodList[i]);
        }

        return resultList;
    }

    public int GetAmount(int id)
    {
        ItemModel data = DataTable.Instance.ItemDic[id];
        if (data.Category == ItemModel.CategoryEnum.Item)
        {
            if (Info.ItemDic.TryGetValue(id, out Item item))
            {
                return item.Amount;
            }
            else
            {
                return 0;
            }
        }
        else if (data.Category == ItemModel.CategoryEnum.Consumables)
        {
            if (Info.ConsumablesDic.TryGetValue(id, out Consumables consumables))
            {
                return consumables.Amount;
            }
            else
            {
                return 0;
            }
        }

        return 0;
    }
}
