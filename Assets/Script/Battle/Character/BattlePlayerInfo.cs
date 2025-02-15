using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerInfo : BattleCharacterInfo
{
    public JobModel Job;

    public BattlePlayerInfo(int lv, JobModel job)
    {
        Job = job;
        Name = job.Name;
        Lv = lv;

        float n = (1 + (lv - 1) * 0.1f);
        MaxHP = Mathf.RoundToInt(job.HP * n);
        STR = Mathf.RoundToInt(job.STR * n);
        CON = Mathf.RoundToInt(job.CON * n);
        INT = Mathf.RoundToInt(job.INT * n);
        MEN = Mathf.RoundToInt(job.MEN * n);
        DEX = Mathf.RoundToInt(job.DEX * n);
        AGI = Mathf.RoundToInt(job.AGI * n);
        MOV = job.MOV;
        WT = job.WT;
        if (job.Passive != -1)
        {
            PassiveList.Add(PassiveFactory.GetPassive(job.Passive));
        }

        Faction = FactionEnum.Player;
        CurrentHP = MaxHP;
        CurrentWT = WT;

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

        if (job.Spell_1 != -1)
        {
            SpellList.Add(new Spell(DataContext.Instance.SpellDic[job.Spell_1]));
        }

        Weapon = new Equip(EquipModel.CategoryEnum.Weapon);
        for (int i = 0; i < job.Armor; i++)
        {
            Armor.Add(new Equip(EquipModel.CategoryEnum.Armor));
        }
        for (int i = 0; i < job.Amulets; i++)
        {
            Decoration.Add(new Equip(EquipModel.CategoryEnum.Amulet));
        }
    }
}
