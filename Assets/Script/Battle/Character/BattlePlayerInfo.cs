using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerInfo : BattleCharacterInfo
{
    public JobModel Job;

    public BattlePlayerInfo(int lv, CharacterInfo info)
    {
        Job = DataContext.Instance.JobDic[info.JobId];
        Name = info.Name;
        FileName = info.Controller;
        Lv = lv;

        MaxHP = info.MaxHP;
        STR = info.STR;
        CON = info.CON;
        INT = info.INT;
        MEN = info.MEN;
        DEX = info.DEX;
        AGI = info.AGI;
        MOV = info.MOV;
        WT = info.WT;
        PassiveList = info.PassiveList;

        Faction = FactionEnum.Player;
        CurrentHP = info.CurrentHP;
        CurrentWT = WT;

        SkillList = info.SkillList;
        SupportList = info.SupportList;

        Weapon = info.Weapon;
        Armor = info.Armor;
        Decoration = info.Decoration;
    }
}
