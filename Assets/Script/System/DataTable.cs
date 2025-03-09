using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTable
{
    private static readonly int _fileCount = 23;

    private static DataTable _instance;
    public static DataTable Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataTable();
            }
            return _instance;
        }
    }

    public List<AttachModel> AttachList = new List<AttachModel>();
    public List<ConsumablesModel> ConsumablesList = new List<ConsumablesModel>();
    public List<ConversationModel> ConversationList = new List<ConversationModel>();
    public List<CookModel> CookList = new List<CookModel>();
    public List<EffectModel> EffectList = new List<EffectModel>();
    public List<EnemyModel> EnemyList = new List<EnemyModel>();
    public List<EnemyGroupModel> EnemyGroupList = new List<EnemyGroupModel>();
    public List<EquipModel> EquipList = new List<EquipModel>();
    public List<FixedFloorModel> FixedFloorList = new List<FixedFloorModel>();
    public List<FoodMaterialModel> FoodMaterialList = new List<FoodMaterialModel>();
    public List<FoodResultModel> FoodResultList = new List<FoodResultModel>();
    public List<ItemModel> ItemList = new List<ItemModel>();
    public List<JobModel> JobList = new List<JobModel>();
    public List<PassiveModel> PassiveList = new List<PassiveModel>();
    public List<RandomFloorModel> RandomFloorList = new List<RandomFloorModel>();
    public List<RoomModel> RoomList = new List<RoomModel>();
    public List<ShopModel> ShopList = new List<ShopModel>();
    public List<SkillModel> SkillList = new List<SkillModel>();
    public List<SpellModel> SpellList = new List<SpellModel>();
    public List<StatusModel> StatusList = new List<StatusModel>();
    public List<SupportModel> SupportList = new List<SupportModel>();
    public List<TileModel> TileList = new List<TileModel>();
    public List<TreasureModel> TreasureList = new List<TreasureModel>();

    public Dictionary<int, AttachModel> AttachDic = new Dictionary<int, AttachModel>();
    public Dictionary<int, ConsumablesModel> ConsumablesDic = new Dictionary<int, ConsumablesModel>();
    public Dictionary<int, Dictionary<int, ConversationModel>> ConversationDic = new Dictionary<int, Dictionary<int, ConversationModel>>();
    public Dictionary<int, EffectModel> EffectDic = new Dictionary<int, EffectModel>();
    public Dictionary<int, EnemyModel> EnemyDic = new Dictionary<int, EnemyModel>();
    public Dictionary<int, EnemyGroupModel> EnemyGroupDic = new Dictionary<int, EnemyGroupModel>();
    public Dictionary<int, EquipModel> EquipDic = new Dictionary<int, EquipModel>();
    public Dictionary<int, FixedFloorModel> FixedFloorDic = new Dictionary<int, FixedFloorModel>();
    public Dictionary<int, FoodMaterialModel> FoodMaterialDic = new Dictionary<int, FoodMaterialModel>();
    public Dictionary<int, FoodResultModel> FoodResultDic = new Dictionary<int, FoodResultModel>();
    public Dictionary<int, ItemModel> ItemDic = new Dictionary<int, ItemModel>();
    public Dictionary<int, JobModel> JobDic = new Dictionary<int, JobModel>();
    public Dictionary<int, PassiveModel> PassiveDic = new Dictionary<int, PassiveModel>();
    public Dictionary<int, RandomFloorModel> RandomFloorDic = new Dictionary<int, RandomFloorModel>();
    public Dictionary<int, RoomModel> RoomDic = new Dictionary<int, RoomModel>();
    public Dictionary<ItemModel.CategoryEnum, List<ShopModel>> ShopItemDic = new Dictionary<ItemModel.CategoryEnum, List<ShopModel>>();
    public Dictionary<int, SkillModel> SkillDic = new Dictionary<int, SkillModel>();
    public Dictionary<int, SpellModel> SpellDic = new Dictionary<int, SpellModel>();
    public Dictionary<int, List<SpellModel>> JobSpellDic = new Dictionary<int, List<SpellModel>>();
    public Dictionary<int, StatusModel> StatusDic = new Dictionary<int, StatusModel>();
    public Dictionary<int, SupportModel> SupportDic = new Dictionary<int, SupportModel>();
    public Dictionary<int, TileModel> TileDic = new Dictionary<int, TileModel>();
    public Dictionary<int, TreasureModel> TreasureDic = new Dictionary<int, TreasureModel>();

    private int _count = 0;
    private Action _callback;
    private FileLoader _fileLoader;

    public void Load(Action callback) 
    {
        _count = 0;
        _callback = callback;
        _fileLoader = GameObject.Find("FileLoader").GetComponent<FileLoader>();

        _fileLoader.Load<List<AttachModel>>("Attach", FileLoader.PathEnum.Data, (obj) =>
        {
            AttachList = (List<AttachModel>)obj;
            AttachDic.Clear();
            for (int i = 0; i < AttachList.Count; i++)
            {
                AttachDic.Add(AttachList[i].ID, AttachList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<ConsumablesModel>>("Consumables", FileLoader.PathEnum.Data, (obj) =>
        {
            ConsumablesList = (List<ConsumablesModel>)obj;
            ConsumablesDic.Clear();
            for (int i = 0; i < ConsumablesList.Count; i++)
            {
                ConsumablesDic.Add(ConsumablesList[i].ID, ConsumablesList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<ConversationModel>>("Conversation", FileLoader.PathEnum.Data, (obj) =>
        {
            ConversationList = (List<ConversationModel>)obj;
            ConversationDic.Clear();
            int id;
            for (int i = 0; i < ConversationList.Count; i++)
            {
                ConversationList[i].GetList();
                id = ConversationList[i].ID;
                if (!ConversationDic.ContainsKey(id))
                {
                    ConversationDic.Add(id, new Dictionary<int, ConversationModel>());
                }
                ConversationDic[id].Add(ConversationList[i].Page, ConversationList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<CookModel>>("Cook", FileLoader.PathEnum.Data, (obj) =>
        {
            CookList = (List<CookModel>)obj;
            for (int i = 0; i < CookList.Count; i++)
            {
                CookList[i].GetList();
            }
            CheckComplete();
        });

        _fileLoader.Load<List<EffectModel>>("Effect", FileLoader.PathEnum.Data, (obj) =>
        {
            EffectList = (List<EffectModel>)obj;
            EffectDic.Clear();
            for (int i = 0; i < EffectList.Count; i++)
            {
                EffectDic.Add(EffectList[i].ID, EffectList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<EnemyModel>>("Enemy", FileLoader.PathEnum.Data, (obj) =>
        {
            EnemyList = (List<EnemyModel>)obj;
            EnemyDic.Clear();
            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i].GetSpriteList();
                EnemyList[i].GetDropList();
                EnemyList[i].GetSkillList();
                EnemyDic.Add(EnemyList[i].ID, EnemyList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<EnemyGroupModel>>("EnemyGroup", FileLoader.PathEnum.Data, (obj) =>
        {
            EnemyGroupList = (List<EnemyGroupModel>)obj;
            EnemyGroupDic.Clear();
            for (int i = 0; i < EnemyGroupList.Count; i++)
            {
                EnemyGroupList[i].GetEnemyList();
                EnemyGroupList[i].GetMapPool();
                EnemyGroupDic.Add(EnemyGroupList[i].ID, EnemyGroupList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<EquipModel>>("Equip", FileLoader.PathEnum.Data, (obj) =>
        {
            EquipList = (List<EquipModel>)obj;
            EquipDic.Clear();
            for (int i = 0; i < EquipList.Count; i++)
            {
                EquipDic.Add(EquipList[i].ID, EquipList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<FixedFloorModel>>("FixedFloor", FileLoader.PathEnum.Data, (obj) =>
        {
            FixedFloorList = (List<FixedFloorModel>)obj;
            FixedFloorDic.Clear();
            for (int i = 0; i < FixedFloorList.Count; i++)
            {
                FixedFloorDic.Add(FixedFloorList[i].Floor, FixedFloorList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<FoodMaterialModel>>("FoodMaterial", FileLoader.PathEnum.Data, (obj) =>
        {
            FoodMaterialList = (List<FoodMaterialModel>)obj;
            FoodMaterialDic.Clear();
            for (int i = 0; i < FoodMaterialList.Count; i++)
            {
                FoodMaterialDic.Add(FoodMaterialList[i].ID, FoodMaterialList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<FoodResultModel>>("FoodResult", FileLoader.PathEnum.Data, (obj) =>
        {
            FoodResultList = (List<FoodResultModel>)obj;
            FoodResultDic.Clear();
            for (int i = 0; i < FoodResultList.Count; i++)
            {
                FoodResultDic.Add(FoodResultList[i].ID, FoodResultList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<ItemModel>>("Item", FileLoader.PathEnum.Data, (obj) =>
        {
            ItemList = (List<ItemModel>)obj;
            ItemDic.Clear();
            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemDic.Add(ItemList[i].ID, ItemList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<JobModel>>("Job", FileLoader.PathEnum.Data, (obj) =>
        {
            JobList = (List<JobModel>)obj;
            JobDic.Clear();
            for (int i = 0; i < JobList.Count; i++)
            {
                JobDic.Add(JobList[i].ID, JobList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<PassiveModel>>("Passive", FileLoader.PathEnum.Data, (obj) =>
        {
            PassiveList = (List<PassiveModel>)obj;
            PassiveDic.Clear();
            for (int i = 0; i < PassiveList.Count; i++)
            {
                PassiveDic.Add(PassiveList[i].ID, PassiveList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<RandomFloorModel>>("RandomFloor", FileLoader.PathEnum.Data, (obj) =>
        {
            RandomFloorList = (List<RandomFloorModel>)obj;
            RandomFloorDic.Clear();
            for (int i = 0; i < RandomFloorList.Count; i++)
            {
                RandomFloorList[i].GetEnemyGroupPool();
                RandomFloorDic.Add(RandomFloorList[i].Floor, RandomFloorList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<RoomModel>>("Room", FileLoader.PathEnum.Data, (obj) =>
        {
            RoomList = (List<RoomModel>)obj;
            RoomDic.Clear();
            for (int i = 0; i < RoomList.Count; i++)
            {
                RoomList[i].GetTreasurePool();
                RoomDic.Add(RoomList[i].ID, RoomList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<ShopModel>>("Shop", FileLoader.PathEnum.Data, (obj) =>
        {
            ShopList = (List<ShopModel>)obj;
            ShopItemDic.Clear();
            ItemModel item;
            for (int i = 0; i < ShopList.Count; i++)
            {
                ShopList[i].GetList();
                item = ItemDic[ShopList[i].ID];
                if (!ShopItemDic.ContainsKey(item.Category))
                {
                    ShopItemDic.Add(item.Category, new List<ShopModel>());
                }
                ShopItemDic[item.Category].Add(ShopList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<SkillModel>>("Skill", FileLoader.PathEnum.Data, (obj) =>
        {
            SkillList = (List<SkillModel>)obj;
            SkillDic.Clear();
            for (int i = 0; i < SkillList.Count; i++)
            {
                SkillDic.Add(SkillList[i].ID, SkillList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<SpellModel>>("Spell", FileLoader.PathEnum.Data, (obj) =>
        {
            SpellList = (List<SpellModel>)obj;
            SpellDic.Clear();
            for (int i = 0; i < SpellList.Count; i++)
            {
                SpellDic.Add(SpellList[i].ID, SpellList[i]);
                if (!JobSpellDic.ContainsKey(SpellList[i].Job))
                {
                    JobSpellDic.Add(SpellList[i].Job, new List<SpellModel>());
                }
                JobSpellDic[SpellList[i].Job].Add(SpellList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<StatusModel>>("Status", FileLoader.PathEnum.Data, (obj) =>
        {
            StatusList = (List<StatusModel>)obj;
            StatusDic.Clear();
            for (int i = 0; i < StatusList.Count; i++)
            {
                StatusList[i].GetAreaList();
                StatusDic.Add(StatusList[i].ID, StatusList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<SupportModel>>("Support", FileLoader.PathEnum.Data, (obj) =>
        {
            SupportList = (List<SupportModel>)obj;
            SupportDic.Clear();
            for (int i = 0; i < SupportList.Count; i++)
            {
                SupportDic.Add(SupportList[i].ID, SupportList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<TileModel>>("Tile", FileLoader.PathEnum.Data, (obj) =>
        {
            TileList = (List<TileModel>)obj;
            TileDic.Clear();
            for (int i = 0; i < TileList.Count; i++)
            {
                TileList[i].GetPool();
                TileDic.Add(TileList[i].ID, TileList[i]);
            }
            CheckComplete();
        });

        _fileLoader.Load<List<TreasureModel>>("Treasure", FileLoader.PathEnum.Data, (obj) =>
        {
            TreasureList = (List<TreasureModel>)obj;
            TreasureDic.Clear();
            for (int i = 0; i < TreasureList.Count; i++)
            {
                TreasureList[i].SetItemList();
                TreasureDic.Add(TreasureList[i].ID, TreasureList[i]);
            }
            CheckComplete();
        });
    }

    private void CheckComplete() 
    {
        _count++;
        if(_count == _fileCount) 
        {
            _callback();
        }
    }
}
