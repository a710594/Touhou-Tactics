using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class CharacterInfo
{
    public string Name;
    public int MaxHP;
    public int CurrentHP;
    public int STR; //Strength �O�q �v�T���z�ˮ`
    public int CON; //Constitution ��� ��ܪ��z�ˮ`
    public int INT; //Intelligence ���O �v�T�k�N�ˮ`
    public int MEN; //mentality �믫 ��ܪk�N�ˮ`
    public int DEX; //Dexterity �F�� �v�T�R���v
    public int AGI; //Agility �ӱ� ��ܩR���v
    public int MOV; //move ����
    public int UP;
    public int DOWN;
    public int WT;
    public string Controller;
    public Equip Weapon = new Equip(EquipModel.CategoryEnum.Weapon);
    public List<Equip> Armor = new List<Equip>();
    public List<Equip> Decoration = new List<Equip>();
    public int Weight;

    [NonSerialized]
    public List<Skill> SkillList = new List<Skill>();
    [NonSerialized]
    public List<Support> SupportList = new List<Support>();
    [NonSerialized]
    public List<Passive> PassiveList = new List<Passive>();
    [NonSerialized]
    public List<Spell> SpellList = new List<Spell>();

    public int JobId;

    public CharacterInfo() { }

    public CharacterInfo(JobModel job)
    {
        Name = job.Name;
        JobId = job.ID;

        int lv = 1;
        float n = (1 + (lv - 1) * 0.1f);
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
        Controller = job.Controller;
        Weight = job.Weight;

        Weapon = new Equip(EquipModel.CategoryEnum.Weapon);
        for (int i = 0; i < job.Armor; i++)
        {
            Armor.Add(new Equip(EquipModel.CategoryEnum.Armor));
        }
        for (int i = 0; i < job.Amulets; i++)
        {
            Decoration.Add(new Equip(EquipModel.CategoryEnum.Amulet));
        }

        Init();
    }

    public void Init() 
    {
        JobModel job = DataTable.Instance.JobDic[JobId];

        if (job.Skill_1 != -1)
        {
            SkillList.Add(new Skill(DataTable.Instance.SkillDic[job.Skill_1]));
        }
        if (job.Skill_2 != -1)
        {
            SkillList.Add(new Skill(DataTable.Instance.SkillDic[job.Skill_2]));
        }
        if (job.Skill_3 != -1)
        {
            SkillList.Add(new Skill(DataTable.Instance.SkillDic[job.Skill_3]));
        }
        if (job.Skill_4 != -1)
        {
            SkillList.Add(new Skill(DataTable.Instance.SkillDic[job.Skill_4]));
        }
        if (job.Skill_5 != -1)
        {
            SkillList.Add(new Skill(DataTable.Instance.SkillDic[job.Skill_5]));
        }

        if (job.Support_1 != -1)
        {
            SupportList.Add(new Support(DataTable.Instance.SupportDic[job.Support_1]));
        }
        if (job.Support_2 != -1)
        {
            SupportList.Add(new Support(DataTable.Instance.SupportDic[job.Support_2]));
        }
        if (job.Support_3 != -1)
        {
            SupportList.Add(new Support(DataTable.Instance.SupportDic[job.Support_3]));
        }
        if (job.Support_4 != -1)
        {
            SupportList.Add(new Support(DataTable.Instance.SupportDic[job.Support_4]));
        }
        if (job.Support_5 != -1)
        {
            SupportList.Add(new Support(DataTable.Instance.SupportDic[job.Support_5]));
        }

        if (job.Passive != -1)
        {
            PassiveList.Add(PassiveFactory.GetPassive(job.Passive));
        }

        if (job.Spell_1 != -1)
        {
            SpellList.Add(new Spell(DataTable.Instance.SpellDic[job.Spell_1]));
        }
    }

    public void SetLv(int lv) 
    {
        JobModel job = DataTable.Instance.JobDic[JobId];
        float n = (1 + (lv - 1) * 0.1f);
        MaxHP = Mathf.RoundToInt(job.HP * n);
        STR = Mathf.RoundToInt(job.STR * n);
        CON = Mathf.RoundToInt(job.CON * n);
        INT = Mathf.RoundToInt(job.INT * n);
        MEN = Mathf.RoundToInt(job.MEN * n);
        DEX = Mathf.RoundToInt(job.DEX * n);
        AGI = Mathf.RoundToInt(job.AGI * n);
    }

    public void Refresh(BattlePlayerInfo info) 
    {
        CurrentHP = info.CurrentHP;
        for (int i=0; i<SkillList.Count; i++) 
        {
            SkillList[i].CurrentCD = 0;
        }

        for (int i = 0; i < SupportList.Count; i++)
        {
            SupportList[i].CurrentCD = 0;
        }
    }

    public void SetRecover(int recover)
    {
        CurrentHP += recover;
        if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }
    }
}
