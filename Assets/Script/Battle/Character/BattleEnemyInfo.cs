using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyInfo : BattleCharacterInfo
{
    public EnemyModel Enemy;

    public BattleEnemyInfo(int lv, EnemyModel enemy)
    {
        Enemy = enemy;
        Name = enemy.Name;
        FileName = enemy.Controller;
        Lv = lv;

        float n = (1 + (lv - 1) * 0.1f);
        MaxHP = Mathf.RoundToInt(enemy.HP * n);
        STR = Mathf.RoundToInt(enemy.STR * n);
        CON = Mathf.RoundToInt(enemy.CON * n);
        INT = Mathf.RoundToInt(enemy.INT * n);
        MEN = Mathf.RoundToInt(enemy.MEN * n);
        DEX = Mathf.RoundToInt(enemy.DEX * n);
        AGI = Mathf.RoundToInt(enemy.AGI * n);
        MOV = enemy.MOV;
        WT = enemy.WT;

        Faction = FactionEnum.Enemy;
        IsAuto = true;
        CurrentHP = MaxHP;
        CurrentWT = WT;

        for (int i = 0; i < enemy.SkillList.Count; i++)
        {
            SkillList.Add(new Skill(DataContext.Instance.SkillDic[enemy.SkillList[i]]));
        }

        Weapon = new Equip(EquipModel.CategoryEnum.Weapon);
        //之後要加上數量
        //for (int i = 0; i < job.Armor; i++)
        //{
        Armor.Add(new Equip(EquipModel.CategoryEnum.Armor));
        //}
        //for (int i = 0; i < job.Amulets; i++)
        //{
        Decoration.Add(new Equip(EquipModel.CategoryEnum.Amulet));
        //}
    }
}
