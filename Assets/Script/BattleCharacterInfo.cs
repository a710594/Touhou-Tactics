using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattleCharacterInfo
{
    public enum FactionEnum 
    {
        Player,
        Enemy,
    }

    //基礎屬性
    public int ID;
    public string Name;
    public int HP;
    public int MP;
    public int PP;
    public int ATK;
    public int DEF;
    public int MTK;
    public int MEF;
    public int SEN;
    public int AGI;
    public int MOV;
    public int UP;
    public int DOWN;
    public int WT;
    public FactionEnum Faction;

    public List<Skill> SkillList = new List<Skill>();

    //當前屬性
    public int CurrentHP;
    public int CurrentMP;
    public int CurrentPP;
    public int CurrentWT;
    public Vector3 Position = new Vector3();
    public Vector3 MoveTo = new Vector3(); //預期要移動到的目標
    public Skill SelectedSkill = null;

    public BattleCharacterInfo(JobModel job) 
    {
        ID = job.ID;
        Name = job.Name;
        HP = job.HP;
        MP = job.MP;
        PP = job.PP;
        ATK = job.ATK;
        DEF = job.DEF;
        MTK = job.MTK;
        MEF = job.MEF;
        SEN = job.SEN;
        AGI = job.AGI;
        MOV = job.MOV;
        UP = job.UP;
        DOWN = job.DOWN;
        WT = job.WT;
        Faction = FactionEnum.Player;

        CurrentHP = HP;
        CurrentMP = MP;
        CurrentPP = PP;
        CurrentWT = WT;
    }

    public BattleCharacterInfo(EnemyModel enemy)
    {
        ID = enemy.ID;
        Name = enemy.Name;
        HP = enemy.HP;
        MP = enemy.MP;
        ATK = enemy.ATK;
        DEF = enemy.DEF;
        MTK = enemy.MTK;
        MEF = enemy.MEF;
        SEN = enemy.SEN;
        AGI = enemy.AGI;
        MOV = enemy.MOV;
        UP = enemy.UP;
        DOWN = enemy.DOWN;
        WT = enemy.WT;
        Faction = FactionEnum.Enemy;

        CurrentHP = HP;
        CurrentMP = MP;
        CurrentPP = PP;
        CurrentWT = WT;
    }

    public void SetDamage(int damage)
    {
        if (CurrentHP > 0)
        {
            CurrentHP -= damage;
        }
        else
        {
            CurrentHP = 0;
        }
    }
}
