using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DataContext
{
    private static readonly string _dataPrePath = Application.streamingAssetsPath + "./Data/";
    private static readonly string _savePrePath = Application.streamingAssetsPath + "./Save/";
    private static readonly string _settingPrePath = Application.streamingAssetsPath + "./Setting/";
    private static readonly string _mapPrePath = Application.streamingAssetsPath + "./Map/";

    private static DataContext _instance;
    public static DataContext Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataContext();
            }
            return _instance;
        }
    }

    public enum PrePathEnum 
    {
        None = -1,
        Data,
        Save,
        Setting,
        Map,
    }

    public List<JobModel> JobList = new List<JobModel>();
    public List<SkillModel> SkillList = new List<SkillModel>();
    public List<EffectModel> EffectList = new List<EffectModel>();
    public List<StatusModel> StatusList = new List<StatusModel>();
    public List<EnemyModel> EnemyList = new List<EnemyModel>();
    public List<SupportModel> SupportList = new List<SupportModel>();
    public List<ItemModel> ItemList = new List<ItemModel>();
    public List<PassiveModel> PassiveList = new List<PassiveModel>();
    public List<EquipModel> EquipList = new List<EquipModel>();
    public List<EnemyGroupModel> EnemyGroupList = new List<EnemyGroupModel>();
    public List<FloorModel> FloorList = new List<FloorModel>();
    public List<RoomModel> RoomList = new List<RoomModel>();
    public List<TreasureModel> TreasureList = new List<TreasureModel>();
    public List<ShopModel> ShopList = new List<ShopModel>();
    public List<CookModel> CookList = new List<CookModel>();
    public List<FoodMaterial> FoodMaterialList = new List<FoodMaterial>();
    public List<FoodResult> FoodResultList = new List<FoodResult>();
    public List<ConsumablesModel> ConsumablesList = new List<ConsumablesModel>();
    public List<CardModel> CardList = new List<CardModel>();

    public Dictionary<int, JobModel> JobDic = new Dictionary<int, JobModel>();
    public Dictionary<int, SkillModel> SkillDic = new Dictionary<int, SkillModel>();
    public Dictionary<int, EffectModel> EffectDic = new Dictionary<int, EffectModel>();
    public Dictionary<int, StatusModel> StatusDic = new Dictionary<int, StatusModel>();
    public Dictionary<int, EnemyModel> EnemyDic = new Dictionary<int, EnemyModel>();
    public Dictionary<int, SupportModel> SupportDic = new Dictionary<int, SupportModel>();
    public Dictionary<int, PassiveModel> PassiveDic = new Dictionary<int, PassiveModel>();
    public Dictionary<int, EquipModel> EquipDic = new Dictionary<int, EquipModel>();
    public Dictionary<int, ItemModel> ItemDic = new Dictionary<int, ItemModel>();
    public Dictionary<int, Dictionary<int, EnemyGroupModel>> EnemyGroupDic = new Dictionary<int, Dictionary<int, EnemyGroupModel>>();
    public Dictionary<int, FloorModel> FloorDic = new Dictionary<int, FloorModel>();
    public Dictionary<int, RoomModel> RoomDic = new Dictionary<int, RoomModel>();
    public Dictionary<int, List<int>> RoomPool = new Dictionary<int, List<int>>(); //機率池 <Floor, idList>
    public Dictionary<int, TreasureModel> TreasureDic = new Dictionary<int, TreasureModel>();
    public Dictionary<ItemModel.CategoryEnum, List<ShopModel>> ShopItemDic = new Dictionary<ItemModel.CategoryEnum, List<ShopModel>>();
    public Dictionary<int, FoodMaterial> FoodMaterialDic = new Dictionary<int, FoodMaterial>();
    public Dictionary<int, FoodResult> FoodResultDic = new Dictionary<int, FoodResult>();
    public Dictionary<int, ConsumablesModel> ConsumablesDic = new Dictionary<int, ConsumablesModel>();
    public Dictionary<int, CardModel> CardDic = new Dictionary<int, CardModel>();

    public Dictionary<string, TileSetting> TileSettingDic = new Dictionary<string, TileSetting>();
    public Dictionary<string, AttachSetting> AttachSettingDic = new Dictionary<string, AttachSetting>();

    public void Init() 
    {
        JobList = Load<List<JobModel>>("Job", PrePathEnum.Data);
        JobDic.Clear();
        for (int i=0; i<JobList.Count; i++) 
        {
            JobDic.Add(JobList[i].ID, JobList[i]);
        }

        SkillList = Load<List<SkillModel>>("Skill", PrePathEnum.Data);
        SkillDic.Clear();
        for (int i = 0; i < SkillList.Count; i++)
        {
            SkillDic.Add(SkillList[i].ID, SkillList[i]);
        }

        EffectList = Load<List<EffectModel>>("Effect", PrePathEnum.Data);
        EffectDic.Clear();
        for (int i=0; i<EffectList.Count; i++) 
        {
            EffectList[i].GetAreaList();
            EffectDic.Add(EffectList[i].ID, EffectList[i]);
        }

        StatusList = Load<List<StatusModel>>("Status", PrePathEnum.Data);
        StatusDic.Clear();
        for (int i = 0; i < StatusList.Count; i++)
        {
            StatusList[i].GetAreaList();
            StatusDic.Add(StatusList[i].ID, StatusList[i]);
        }

        EnemyList = Load<List<EnemyModel>>("Enemy", PrePathEnum.Data);
        EnemyDic.Clear();
        for (int i = 0; i < EnemyList.Count; i++)
        {
            EnemyList[i].GetDropList();
            EnemyDic.Add(EnemyList[i].ID, EnemyList[i]);
        }

        SupportList = Load<List<SupportModel>>("Support", PrePathEnum.Data);
        SupportDic.Clear();
        for (int i = 0; i < SupportList.Count; i++)
        {
            SupportDic.Add(SupportList[i].ID, SupportList[i]);
        }

        ItemList = Load<List<ItemModel>>("Item", PrePathEnum.Data);
        ItemDic.Clear();
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemDic.Add(ItemList[i].ID, ItemList[i]);
        }

        PassiveList = Load<List<PassiveModel>>("Passive", PrePathEnum.Data);
        PassiveDic.Clear();
        for (int i = 0; i < PassiveList.Count; i++)
        {
            PassiveDic.Add(PassiveList[i].ID, PassiveList[i]);
        }

        EquipList = Load<List<EquipModel>>("Equip", PrePathEnum.Data);
        EquipDic.Clear();
        for (int i = 0; i < EquipList.Count; i++)
        {
            EquipDic.Add(EquipList[i].ID, EquipList[i]);
        }

        EnemyGroupList = Load<List<EnemyGroupModel>>("EnemyGroup", PrePathEnum.Data);
        EnemyGroupDic.Clear();
        for (int i = 0; i < EnemyGroupList.Count; i++)
        {
            EnemyGroupList[i].GetEnemyList();
            if (!EnemyGroupDic.ContainsKey(EnemyGroupList[i].Floor))
            {
                EnemyGroupDic.Add(EnemyGroupList[i].Floor, new Dictionary<int, EnemyGroupModel>());
            }
            EnemyGroupDic[EnemyGroupList[i].Floor].Add(EnemyGroupList[i].ID, EnemyGroupList[i]);
        }

        FloorList = Load<List<FloorModel>>("Floor", PrePathEnum.Data);
        FloorDic.Clear();
        for (int i = 0; i < FloorList.Count; i++)
        {
            FloorDic.Add(FloorList[i].Floor, FloorList[i]);
        }

        RoomList = Load<List<RoomModel>>("Room", PrePathEnum.Data);
        RoomDic.Clear();
        for (int i = 0; i < RoomList.Count; i++)
        {
            RoomDic.Add(RoomList[i].ID, RoomList[i]);
        }

        RoomPool.Clear();
        for (int i=0; i< FloorList.Count; i++) 
        {
            RoomPool.Add(FloorList[i].Floor, new List<int>());
            if (FloorList[i].Room_1 != -1) 
            {
                for (int j = 0; j < FloorList[i].Probability_1; j++)
                {
                    RoomPool[FloorList[i].Floor].Add(FloorList[i].Room_1);
                }

                for (int j = 0; j < FloorList[i].Probability_2; j++)
                {
                    RoomPool[FloorList[i].Floor].Add(FloorList[i].Room_2);
                }

                for (int j = 0; j < FloorList[i].Probability_3; j++)
                {
                    RoomPool[FloorList[i].Floor].Add(FloorList[i].Room_3);
                }

                for (int j = 0; j < FloorList[i].Probability_4; j++)
                {
                    RoomPool[FloorList[i].Floor].Add(FloorList[i].Room_4);
                }

                for (int j = 0; j < FloorList[i].Probability_5; j++)
                {
                    RoomPool[FloorList[i].Floor].Add(FloorList[i].Room_5);
                }
            }
        }

        TreasureList = Load<List<TreasureModel>>("Treasure", PrePathEnum.Data);
        TreasureDic.Clear();
        for (int i = 0; i < TreasureList.Count; i++)
        {
            TreasureList[i].GetList();
            TreasureDic.Add(TreasureList[i].ID, TreasureList[i]);
        }

        ShopList = Load<List<ShopModel>>("Shop", PrePathEnum.Data);
        ItemModel item;
        ShopItemDic.Clear();
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

        CookList = Load<List<CookModel>>("Cook", PrePathEnum.Data);
        for (int i=0; i<CookList.Count; i++) 
        {
            CookList[i].GetList();
        }

        FoodMaterialList = Load<List<FoodMaterial>>("FoodMaterial", PrePathEnum.Data);
        FoodMaterialDic.Clear();
        for (int i = 0; i < FoodMaterialList.Count; i++)
        {
            FoodMaterialDic.Add(FoodMaterialList[i].ID, FoodMaterialList[i]);
        }

        FoodResultList = Load<List<FoodResult>>("FoodResult", PrePathEnum.Data);
        FoodResultDic.Clear();
        for (int i = 0; i < FoodResultList.Count; i++)
        {
            FoodResultDic.Add(FoodResultList[i].ID, FoodResultList[i]);
        }

        ConsumablesList = Load<List<ConsumablesModel>>("Consumables", PrePathEnum.Data);
        ConsumablesDic.Clear();
        for (int i = 0; i < ConsumablesList.Count; i++)
        {
            ConsumablesDic.Add(ConsumablesList[i].ID, ConsumablesList[i]);
        }

        CardList = Load<List<CardModel>>("Card", PrePathEnum.Data);
        CardDic.Clear();
        for (int i = 0; i < CardList.Count; i++)
        {
            CardDic.Add(CardList[i].ID, CardList[i]);
        }

        DirectoryInfo d;
        FileInfo[] Files;
        string str = ""; 
        string jsonString;
        AttachSetting attach;
        TileSetting tile;

        d = new DirectoryInfo(Application.streamingAssetsPath + "./Attach/"); //Assuming Test is your Folder
        Files = d.GetFiles("*.json"); //Getting Text files
        AttachSettingDic.Clear();
        foreach (FileInfo file in Files)
        {
            str = str + ", " + file.Name;
            jsonString = File.ReadAllText(file.FullName);
            attach = JsonConvert.DeserializeObject<AttachSetting>(jsonString);
            AttachSettingDic.Add(attach.ID, attach);

        }

        d = new DirectoryInfo(Application.streamingAssetsPath + "./Tile/"); //Assuming Test is your Folder
        Files = d.GetFiles("*.json"); //Getting Text files
        TileSettingDic.Clear();
        foreach (FileInfo file in Files)
        {
            str = str + ", " + file.Name;
            jsonString = File.ReadAllText(file.FullName);
            tile = JsonConvert.DeserializeObject<TileSetting>(jsonString);
            TileSettingDic.Add(tile.ID, tile);

        }
    }

    public T Load<T>(string fileName, PrePathEnum prePathEnum)
    {
        try
        {
            string path;
            string prePath = "";
            if(prePathEnum == PrePathEnum.Data) 
            {
                prePath = _dataPrePath;
            }
            else if(prePathEnum == PrePathEnum.Save) 
            {
                prePath = _savePrePath;
            }
            else if (prePathEnum == PrePathEnum.Setting)
            {
                prePath = _settingPrePath;
            }
            else if (prePathEnum == PrePathEnum.Map)
            {
                prePath = _mapPrePath;
            }

            if (fileName == "")
            {
                path = Path.Combine(prePath, typeof(T).Name + ".json");
            }
            else
            {
                path = Path.Combine(prePath, fileName + ".json");
            }
            string jsonString = File.ReadAllText(path);
            T info = JsonConvert.DeserializeObject<T>(jsonString);
            return info;
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
            return default(T);
        }
    }

    public void Save<T>(T t, string fileName, PrePathEnum prePathEnum)
    {
        try
        {
            string path;
            string prePath = "";
            if (prePathEnum == PrePathEnum.Data)
            {
                prePath = _dataPrePath;
            }
            else if (prePathEnum == PrePathEnum.Save)
            {
                prePath = _savePrePath;
            }
            else if (prePathEnum == PrePathEnum.Setting)
            {
                prePath = _settingPrePath;
            }
            else if (prePathEnum == PrePathEnum.Map)
            {
                prePath = _mapPrePath;
            }

            if (fileName == "")
            {
                path = Path.Combine(prePath, typeof(T).Name + ".json");
            }
            else
            {
                path = Path.Combine(prePath, fileName + ".json");
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(t));
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    public void DeleteData<T>()
    {
        string path = Path.Combine(_dataPrePath, typeof(T).Name + ".json");
        File.Delete(path);
    }

    public void DeleteData(string fileName = "")
    {
        File.Delete(Path.Combine(_dataPrePath, fileName + ".json"));
    }
}