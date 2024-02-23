using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class ItemManager
{
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

    public BagInfo BagInfo;

    public void Init()
    {
        Load();
    }

    public void Load()
    {
        BagInfo baginfo = DataContext.Instance.Load<BagInfo>(_fileName, DataContext.PrePathEnum.Save);
        if (baginfo != null)
        {
            BagInfo = baginfo;

            foreach(KeyValuePair<int, Consumables> pair in baginfo.ConsumablesDic)
            {
                pair.Value.Init();
            }

            foreach (KeyValuePair<int, Card> pair in baginfo.CardDic)
            {
                pair.Value.Init();
            }

            foreach (KeyValuePair<int, Item> pair in baginfo.ItemDic)
            {
                pair.Value.Init();
            }

            for (int i=0; i<baginfo.FoodList.Count; i++) 
            {
                baginfo.FoodList[i].Init();
            }
        }
        else
        {
            BagInfo = new BagInfo();
        }
    }

    public void Save()
    {
        DataContext.Instance.Save(BagInfo, _fileName, DataContext.PrePathEnum.Save);
    }

    public void Delete()
    {
        DataContext.Instance.DeleteData(_fileName);
    }

    public void AddItem(int id, int amount)
    {
        ItemModel data = DataContext.Instance.ItemDic[id];

        if (data.Category == ItemModel.CategoryEnum.Item)
        {
            if (!BagInfo.ItemDic.ContainsKey(id))
            {
                Item item = new Item(id, amount);
                BagInfo.ItemDic.Add(id, item);
            }
            else
            {
                BagInfo.ItemDic[id].Amount += amount;
            }
        }
        else if (data.Category == ItemModel.CategoryEnum.Consumables)
        {
            if (!BagInfo.ConsumablesDic.ContainsKey(id))
            {
                Consumables consumables = new Consumables(id, amount);
                BagInfo.ConsumablesDic.Add(id, consumables);
            }
            else
            {
                BagInfo.ConsumablesDic[id].Amount += amount;
            }
        }
        else if (data.Category == ItemModel.CategoryEnum.Card)
        {
            if (!BagInfo.CardDic.ContainsKey(id))
            {
                Card card = new Card(id, amount);
                BagInfo.CardDic.Add(id, card);
            }
            else
            {
                BagInfo.CardDic[id].Amount += amount;
            }
        }
    }

    public bool MinusItem(int id, int amount)
    {
        ItemModel data = DataContext.Instance.ItemDic[id];
        if (data.Category == ItemModel.CategoryEnum.Item)
        {
            Item item = BagInfo.ItemDic[id];
            if (amount <= item.Amount)
            {
                item.Amount -= amount;
                if (item.Amount == 0)
                {
                    BagInfo.ItemDic.Remove(item.Data.ID);
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
            Consumables consumables = BagInfo.ConsumablesDic[id];
            if (amount <= consumables.Amount)
            {
                consumables.Amount -= amount;
                if (consumables.Amount == 0)
                {
                    BagInfo.ConsumablesDic.Remove(consumables.ItemData.ID);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (data.Category == ItemModel.CategoryEnum.Card)
        {
            Card card = BagInfo.CardDic[id];
            if (amount <= card.Amount)
            {
                card.Amount -= amount;
                if (card.Amount == 0)
                {
                    BagInfo.CardDic.Remove(card.ItemData.ID);
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
        if (BagInfo.EquipList.Count < _maxEquipCount)
        {
            Equip equip = new Equip(id);
            BagInfo.EquipList.Add(equip);

            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddEquip(Equip equip) 
    {
        BagInfo.EquipList.Add(equip);
    }

    public void MinusEquip(Equip equip)
    {
        BagInfo.EquipList.Remove(equip);
    }

    public void AddFood(Food food) 
    {
        BagInfo.FoodList.Add(food);
    }

    public void MinusFood(Food food)
    {
        BagInfo.FoodList.Remove(food);
    }

    public List<object> GetBattleItemList(BattleCharacterInfo character)
    {
        List<object> resultList = new List<object>();

        foreach (KeyValuePair<int, Card> pair in BagInfo.CardDic)
        {
            if (pair.Value.CardData.Job == -1 || (character.Job != null && pair.Value.CardData.Job == character.Job.ID))
            {
                resultList.Add(pair.Value);
            }
        }

        foreach (KeyValuePair<int, Consumables> pair in BagInfo.ConsumablesDic) 
        {
            resultList.Add(pair.Value);
        }

        for(int i=0; i<BagInfo.FoodList.Count; i++)
        {
            resultList.Add(BagInfo.FoodList[i]);
        }

        return resultList;
    }

    public int GetAmount(ItemModel.CategoryEnum category, int id)
    {
        if (category == ItemModel.CategoryEnum.Item)
        {
            if (BagInfo.ItemDic.TryGetValue(id, out Item item))
            {
                return item.Amount;
            }
            else
            {
                return 0;
            }
        }
        else if (category == ItemModel.CategoryEnum.Consumables)
        {
            if (BagInfo.ConsumablesDic.TryGetValue(id, out Consumables consumables))
            {
                return consumables.Amount;
            }
            else
            {
                return 0;
            }
        }
        else if (category == ItemModel.CategoryEnum.Card)
        {
            if (BagInfo.CardDic.TryGetValue(id, out Card card))
            {
                return card.Amount;
            }
            else
            {
                return 0;
            }
        }

        return 0;
    }
}
