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
        Data,
        Save,
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

    public Dictionary<int, JobModel> JobDic = new Dictionary<int, JobModel>();
    public Dictionary<int, SkillModel> SkillDic = new Dictionary<int, SkillModel>();
    public Dictionary<EffectModel.TypeEnum, Dictionary<int, EffectModel>> EffectDic = new Dictionary<EffectModel.TypeEnum, Dictionary<int, EffectModel>>();
    public Dictionary<StatusModel.TypeEnum, Dictionary<int, StatusModel>> StatusDic = new Dictionary<StatusModel.TypeEnum, Dictionary<int, StatusModel>>();
    public Dictionary<int, EnemyModel> EnemyDic = new Dictionary<int, EnemyModel>();
    public Dictionary<int, SupportModel> SupportDic = new Dictionary<int, SupportModel>();
    public Dictionary<int, PassiveModel> PassiveDic = new Dictionary<int, PassiveModel>();
    public Dictionary<EquipModel.CategoryEnum, Dictionary<int, EquipModel>> EquipDic = new Dictionary<EquipModel.CategoryEnum, Dictionary<int, EquipModel>>();
    public Dictionary<ItemModel.CategoryEnum, Dictionary<int, ItemModel>> ItemDic = new Dictionary<ItemModel.CategoryEnum, Dictionary<int, ItemModel>>();
    public Dictionary<int, Dictionary<int, EnemyGroupModel>> EnemyGroupDic = new Dictionary<int, Dictionary<int, EnemyGroupModel>>();
    
    public Dictionary<string, TileScriptableObject> TileScriptableObjectDic = new Dictionary<string, TileScriptableObject>();
    public Dictionary<string, AttachScriptableObject> AttachScriptableObjectDic = new Dictionary<string, AttachScriptableObject>();

    public void Init() 
    {
        JobList = Load<List<JobModel>>("Job", PrePathEnum.Data);
        for (int i=0; i<JobList.Count; i++) 
        {
            JobDic.Add(JobList[i].ID, JobList[i]);
        }

        SkillList = Load<List<SkillModel>>("Skill", PrePathEnum.Data);
        for (int i = 0; i < SkillList.Count; i++)
        {
            SkillDic.Add(SkillList[i].ID, SkillList[i]);
        }

        EffectList = Load<List<EffectModel>>("Effect", PrePathEnum.Data);
        for (int i=0; i<EffectList.Count; i++) 
        {
            EffectList[i].GetAreaList();
            if (!EffectDic.ContainsKey(EffectList[i].Type)) 
            {
                EffectDic.Add(EffectList[i].Type, new Dictionary<int, EffectModel>());
            }
            EffectDic[EffectList[i].Type].Add(EffectList[i].ID, EffectList[i]);
        }

        StatusList = Load<List<StatusModel>>("Status", PrePathEnum.Data);
        for (int i = 0; i < StatusList.Count; i++)
        {
            StatusList[i].GetAreaList();
            if (!StatusDic.ContainsKey(StatusList[i].Type))
            {
                StatusDic.Add(StatusList[i].Type, new Dictionary<int, StatusModel>());
            }
            StatusDic[StatusList[i].Type].Add(StatusList[i].ID, StatusList[i]);
        }

        EnemyList = Load<List<EnemyModel>>("Enemy", PrePathEnum.Data);
        for (int i = 0; i < EnemyList.Count; i++)
        {
            EnemyList[i].GetDropList();
            EnemyDic.Add(EnemyList[i].ID, EnemyList[i]);
        }

        SupportList = Load<List<SupportModel>>("Support", PrePathEnum.Data);
        for (int i = 0; i < SupportList.Count; i++)
        {
            SupportDic.Add(SupportList[i].ID, SupportList[i]);
        }

        ItemList = Load<List<ItemModel>>("Item", PrePathEnum.Data);
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (!ItemDic.ContainsKey(ItemList[i].Category))
            {
                ItemDic.Add(ItemList[i].Category, new Dictionary<int, ItemModel>());
            }
            ItemDic[ItemList[i].Category].Add(ItemList[i].ID, ItemList[i]);
        }

        PassiveList = Load<List<PassiveModel>>("Passive", PrePathEnum.Data);
        for (int i = 0; i < PassiveList.Count; i++)
        {
            PassiveDic.Add(PassiveList[i].ID, PassiveList[i]);
        }

        EquipList = Load<List<EquipModel>>("Equip", PrePathEnum.Data);
        for (int i = 0; i < EquipList.Count; i++)
        {
            if (!EquipDic.ContainsKey(EquipList[i].Category))
            {
                EquipDic.Add(EquipList[i].Category, new Dictionary<int, EquipModel>());
            }
            EquipDic[EquipList[i].Category].Add(EquipList[i].ID, EquipList[i]);
        }

        EnemyGroupList = Load<List<EnemyGroupModel>>("EnemyGroup", PrePathEnum.Data);
        for (int i = 0; i < EnemyGroupList.Count; i++)
        {
            EnemyGroupList[i].GetEnemyList();
            if (!EnemyGroupDic.ContainsKey(EnemyGroupList[i].Floor))
            {
                EnemyGroupDic.Add(EnemyGroupList[i].Floor, new Dictionary<int, EnemyGroupModel>());
            }
            EnemyGroupDic[EnemyGroupList[i].Floor].Add(EnemyGroupList[i].ID, EnemyGroupList[i]);
        }

        DirectoryInfo d;
        FileInfo[] Files;
        string str = ""; 
        string jsonString;
        AttachScriptableObject attach;
        TileScriptableObject tile;

        d = new DirectoryInfo(Application.streamingAssetsPath + "./Attach/"); //Assuming Test is your Folder
        Files = d.GetFiles("*.json"); //Getting Text files
        foreach (FileInfo file in Files)
        {
            str = str + ", " + file.Name;
            jsonString = File.ReadAllText(file.FullName);
            attach = JsonConvert.DeserializeObject<AttachScriptableObject>(jsonString);
            AttachScriptableObjectDic.Add(attach.ID, attach);

        }

        d = new DirectoryInfo(Application.streamingAssetsPath + "./Tile/"); //Assuming Test is your Folder
        Files = d.GetFiles("*.json"); //Getting Text files
        foreach (FileInfo file in Files)
        {
            str = str + ", " + file.Name;
            jsonString = File.ReadAllText(file.FullName);
            tile = JsonConvert.DeserializeObject<TileScriptableObject>(jsonString);
            TileScriptableObjectDic.Add(tile.ID, tile);

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