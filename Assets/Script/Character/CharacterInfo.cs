using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo
{
    public int ID;
    public string Name;
    public int Lv;
    public int MaxHP;
    public int CurrentHP;
    public int STR; //Strength 力量 影響物理傷害
    public int CON; //Constitution 體質 抵抗物理傷害
    public int INT; //Intelligence 智力 影響法術傷害
    public int MEN; //mentality 精神 抵抗法術傷害
    public int DEX; //Dexterity 靈巧 影響命中率
    public int AGI; //Agility 敏捷 對抗命中率
    public int MOV; //move 移動
    public int UP;
    public int DOWN;
    public int WT;
    public string Controller;
    public Equip Weapon = new Equip(EquipModel.CategoryEnum.Weapon);
    public Equip Armor = new Equip(EquipModel.CategoryEnum.Armor);
    public Equip[] Amulets = new Equip[3] { new Equip(EquipModel.CategoryEnum.Amulet), new Equip(EquipModel.CategoryEnum.Amulet), new Equip(EquipModel.CategoryEnum.Amulet) };

    public List<Skill> SkillList = new List<Skill>();
    public List<Support> SupportList = new List<Support>();
    public List<Passive> PassiveList = new List<Passive>();

    public CharacterInfo() { }

    public CharacterInfo(JobModel job)
    {
        ID = job.ID;
        Name = job.Name;
        Lv = 1;

        float n = (1 + (Lv - 1) * 0.1f);
        MaxHP = Mathf.RoundToInt(job.HP * n);
        CurrentHP = MaxHP;
        STR = Mathf.RoundToInt(job.STR * n);
        CON = Mathf.RoundToInt(job.CON * n);
        INT = Mathf.RoundToInt(job.INT * n);
        MEN = Mathf.RoundToInt(job.MEN * n);
        DEX = Mathf.RoundToInt(job.DEX * n);
        AGI = Mathf.RoundToInt(job.AGI * n);
        MOV = job.MOV;
        UP = job.UP;
        DOWN = job.DOWN;
        WT = job.WT;
        if (job.Passive != -1)
        {
            PassiveList.Add(PassiveFactory.GetPassive(job.Passive));
        }
        Controller = job.Controller;

        if (job.Skill_1 != -1)
        {
            SkillList.Add(new Skill(DataContext.Instance.SkillDic[job.Skill_1]));
        }
        if (job.Skill_2 != -1)
        {
            SkillList.Add(new Skill(DataContext.Instance.SkillDic[job.Skill_2]));
        }
        if (job.Skill_3 != -1)
        {
            SkillList.Add(new Skill(DataContext.Instance.SkillDic[job.Skill_3]));
        }
        if (job.Skill_4 != -1)
        {
            SkillList.Add(new Skill(DataContext.Instance.SkillDic[job.Skill_4]));
        }
        if (job.Skill_5 != -1)
        {
            SkillList.Add(new Skill(DataContext.Instance.SkillDic[job.Skill_5]));
        }

        if (job.Support_1 != -1)
        {
            SupportList.Add(new Support(DataContext.Instance.SupportDic[job.Support_1]));
        }
        if (job.Support_2 != -1)
        {
            SupportList.Add(new Support(DataContext.Instance.SupportDic[job.Support_2]));
        }
        if (job.Support_3 != -1)
        {
            SupportList.Add(new Support(DataContext.Instance.SupportDic[job.Support_3]));
        }
        if (job.Support_4 != -1)
        {
            SupportList.Add(new Support(DataContext.Instance.SupportDic[job.Support_4]));
        }
        if (job.Support_5 != -1)
        {
            SupportList.Add(new Support(DataContext.Instance.SupportDic[job.Support_5]));
        }
    }
}
