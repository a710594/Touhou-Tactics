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
    public List<EnemyModel> EnemyList = new List<EnemyModel>();

    public Dictionary<int, JobModel> JobDic = new Dictionary<int, JobModel>();
    public Dictionary<int, SkillModel> SkillDic = new Dictionary<int, SkillModel>();
    public Dictionary<EffectModel.TypeEnum, Dictionary<int, EffectModel>> EffectDic = new Dictionary<EffectModel.TypeEnum, Dictionary<int, EffectModel>>();
    public Dictionary<int, EnemyModel> EnemyDic = new Dictionary<int, EnemyModel>();

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
            SkillDic.Add(SkillList[i].ID, SkillList[i]);
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

        EnemyList = Load<List<EnemyModel>>("Enemy");
        for (int i = 0; i < EnemyList.Count; i++)
        {
            EnemyDic.Add(EnemyList[i].ID, EnemyList[i]);
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