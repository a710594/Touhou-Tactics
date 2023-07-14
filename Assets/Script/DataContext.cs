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

    public List<JobModel> JobList = new List<JobModel>();
    public List<SkillModel> SkillList = new List<SkillModel>();
    public List<EffectModel> EffectList = new List<EffectModel>();
    public List<StatusModel> StatusList = new List<StatusModel>();
    public List<EnemyModel> EnemyList = new List<EnemyModel>();
    public List<SupportModel> SupportList = new List<SupportModel>();

    public Dictionary<int, JobModel> JobDic = new Dictionary<int, JobModel>();
    public Dictionary<SkillModel.TypeEnum, Dictionary<int, SkillModel>> SkillDic = new Dictionary<SkillModel.TypeEnum, Dictionary<int, SkillModel>>();
    public Dictionary<EffectModel.TypeEnum, Dictionary<int, EffectModel>> EffectDic = new Dictionary<EffectModel.TypeEnum, Dictionary<int, EffectModel>>();
    public Dictionary<StatusModel.TypeEnum, Dictionary<int, StatusModel>> StatusDic = new Dictionary<StatusModel.TypeEnum, Dictionary<int, StatusModel>>();
    public Dictionary<int, EnemyModel> EnemyDic = new Dictionary<int, EnemyModel>();
    public Dictionary<int, SupportModel> SupportDic = new Dictionary<int, SupportModel>();
    public Dictionary<string, TileScriptableObject> TileScriptableObjectDic = new Dictionary<string, TileScriptableObject>();
    public Dictionary<string, AttachScriptableObject> AttachScriptableObjectDic = new Dictionary<string, AttachScriptableObject>();

    private string _prePath = Application.streamingAssetsPath + "./Data/";

    public void Init() 
    {
        JobList = Load<List<JobModel>>("Job");
        for (int i=0; i<JobList.Count; i++) 
        {
            JobDic.Add(JobList[i].ID, JobList[i]);
        }

        SkillList = Load<List<SkillModel>>("Skill");
        for (int i = 0; i < SkillList.Count; i++)
        {
            if (!SkillDic.ContainsKey(SkillList[i].Type))
            {
                SkillDic.Add(SkillList[i].Type, new Dictionary<int, SkillModel>());
            }
            SkillDic[SkillList[i].Type].Add(SkillList[i].ID, SkillList[i]);
        }

        EffectList = Load<List<EffectModel>>("Effect");
        for (int i=0; i<EffectList.Count; i++) 
        {
            EffectList[i].GetAreaList();
            if (!EffectDic.ContainsKey(EffectList[i].Type)) 
            {
                EffectDic.Add(EffectList[i].Type, new Dictionary<int, EffectModel>());
            }
            EffectDic[EffectList[i].Type].Add(EffectList[i].ID, EffectList[i]);
        }

        StatusList = Load<List<StatusModel>>("Status");
        for (int i = 0; i < StatusList.Count; i++)
        {
            StatusList[i].GetAreaList();
            if (!StatusDic.ContainsKey(StatusList[i].Type))
            {
                StatusDic.Add(StatusList[i].Type, new Dictionary<int, StatusModel>());
            }
            StatusDic[StatusList[i].Type].Add(StatusList[i].ID, StatusList[i]);
        }

        EnemyList = Load<List<EnemyModel>>("Enemy");
        for (int i = 0; i < EnemyList.Count; i++)
        {
            EnemyDic.Add(EnemyList[i].ID, EnemyList[i]);
        }

        SupportList = Load<List<SupportModel>>("Support");
        for (int i = 0; i < SupportList.Count; i++)
        {
            SupportDic.Add(SupportList[i].ID, SupportList[i]);
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

    public T Load<T>(string fileName = "")
    {
        try
        {
            string path;
            if (fileName == "")
            {
                path = Path.Combine(_prePath, typeof(T).Name + ".json");
            }
            else
            {
                path = Path.Combine(_prePath, fileName + ".json");
            }
            string jsonString = File.ReadAllText(path);
            T info = JsonConvert.DeserializeObject<T>(jsonString);
            return info;
        }
        catch (Exception ex)
        {
            return default(T);
        }
    }
}