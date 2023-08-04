using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    private readonly string _fileName = "BagInfo";

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

    public int Money
    {
        get
        {
            return _bagInfo.Money;
        }
        set
        {
            _bagInfo.Money = value;
        }
    }

    public Dictionary<ItemModel.CategoryEnum, Dictionary<int, Item>> ItemDic = new Dictionary<ItemModel.CategoryEnum, Dictionary<int, Item>>() //<Category, <ID, Item>>
    { { ItemModel.CategoryEnum.Medicine, new Dictionary<int, Item>()} , { ItemModel.CategoryEnum.Card, new Dictionary<int, Item>()} }; 

    private BagInfo _bagInfo;

    public void Init()
    {
        Load();
    }

    public void Load()
    {
        ItemDic.Clear();

        BagInfo baginfo = DataContext.Instance.Load<BagInfo>(_fileName);
        if (baginfo != null)
        {
            _bagInfo = baginfo;

            for (int i = 0; i < baginfo.ItemList.Count; i++)
            {
                baginfo.ItemList[i].Data = DataContext.Instance.ItemDic[baginfo.ItemList[i].Category][baginfo.ItemList[i].ID];
                baginfo.ItemList[i].Effect = EffectFactory.GetEffect(baginfo.ItemList[i].Data.EffectType, baginfo.ItemList[i].Data.EffectID);
                ItemDic[baginfo.ItemList[i].Data.Category].Add(baginfo.ItemList[i].ID, baginfo.ItemList[i]);
            }
        }
        else
        {
            _bagInfo = new BagInfo();
        }
    }

    public void Save()
    {
        _bagInfo.Save(ItemDic);
        DataContext.Instance.Save(_bagInfo, _fileName);
    }

    public void Delete()
    {
        DataContext.Instance.DeleteData(_fileName);
    }

    public void AddItem(ItemModel.CategoryEnum category, int id, int amount)
    {
        Item item;

        if (!ItemDic.ContainsKey(category))
        {
            ItemDic.Add(category, new Dictionary<int, Item>());
        }

        if (!ItemDic[category].ContainsKey(id))
        {
            item = new Item(category, id, amount);
            ItemDic[category].Add(id, item);
        }
        else
        {
            ItemDic[category][id].Amount += amount;
        }
    }

    public bool MinusItem(Item item, int amount)
    {
        if (amount <= item.Amount)
        {
            item.Amount -= amount;
            if (item.Amount == 0)
            {
                ItemDic[item.Data.Category].Remove(item.Data.ID);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Item> GetItemList(ItemModel.CategoryEnum category)
    {
        Dictionary<int, Item> dic = null;
        List<Item> list = new List<Item>();
        if (ItemDic.TryGetValue(category, out dic))
        {
            list = new List<Item>(dic.Values);
        }

        return list;
    }

    public List<Item> GetItemList(BattleCharacterInfo character)
    {
        Dictionary<int, Item> dic = null;
        List<Item> cardList = new List<Item>();
        List<Item> medicineList = new List<Item>();
        List<Item> resultList = new List<Item>();

        if (ItemDic.TryGetValue(ItemModel.CategoryEnum.Card, out dic))
        {
            cardList = new List<Item>(dic.Values);
            for (int i=0; i<cardList.Count; i++) 
            {
                if(cardList[i].Data.Job==-1 ||(character.Job!= null && cardList[i].Data.Job == character.Job.ID)) 
                {
                    resultList.Add(cardList[i]);
                }
            }
        }

        if (ItemDic.TryGetValue(ItemModel.CategoryEnum.Medicine, out dic))
        {
            medicineList = new List<Item>(dic.Values);
            for (int i=0; i<medicineList.Count; i++) 
            {
                resultList.Add(medicineList[i]);
            }
        }

        return medicineList;
    }

    public Item GetItem(ItemModel.CategoryEnum category, int id)
    {
        Dictionary<int, Item> dic = null;
        Item item = null;
        if (ItemDic.TryGetValue(category, out dic))
        {
            dic.TryGetValue(id, out item);
        }
        return item;
    }
}
