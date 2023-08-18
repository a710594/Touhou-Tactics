using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattleCharacterInfo
{
    public static readonly int MaxPP = 10;
    public static readonly Vector3 DefaultLastPosition = new (int.MaxValue, int.MaxValue, int.MaxValue); 

    public enum FactionEnum 
    {
        Player,
        Enemy,
    }

    //基礎屬性
    public int ID;
    public string Name;
    public int MaxHP;
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
    public FactionEnum Faction;
    public JobModel Job = null;
    public EnemyModel Enemy = null;
    public Equip Weapon = new Equip(EquipModel.CategoryEnum.Weapon);
    public Equip Armor = new Equip(EquipModel.CategoryEnum.Armor);
    public Equip[] Amulets = new Equip[3] { new Equip(EquipModel.CategoryEnum.Amulet), new Equip(EquipModel.CategoryEnum.Amulet) , new Equip(EquipModel.CategoryEnum.Amulet) };

    public List<Skill> SkillList = new List<Skill>();
    public List<Support> SupportList = new List<Support>();

    //當前屬性
    public bool IsAuto = false;
    public bool HasUseSkill = false;
    public bool HasUseSupport = false;
    public bool HasUseItem = false;
    public int CurrentHP;
    public int CurrentPP = 0; //符卡點數
    public int CurrentWT;
    public int CurrentSP = 2; //support point
    public int ActionCount = 2; //每個角色都有兩次的行動機會
    public Vector3 Position = new Vector3();
    public Vector3 LastPosition = new Vector3();
    public Skill SelectedSkill = null;
    public Support SelectedSupport = null;
    public Item SelectedItem = null;
    public AI AI = null;
    public Vector2Int Direction = Vector2Int.left;
    public List<Status> StatusList = new List<Status>();
    public List<Passive> PassiveList = new List<Passive>();

    public BattleCharacterInfo(JobModel job) 
    {
        Job = job;
        ID = job.ID;
        Name = job.Name;
        MaxHP = job.HP;
        STR = job.STR;
        CON = job.CON;
        INT = job.INT;
        MEN = job.MEN;
        DEX = job.DEX;
        AGI = job.AGI;
        MOV = job.MOV;
        UP = job.UP;
        DOWN = job.DOWN;
        WT = job.WT;
        if(job.Passive != -1) 
        {
            PassiveList.Add(PassiveFactory.GetPassive(job.Passive));
        }
        Controller = job.Controller;
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
    }

    public BattleCharacterInfo(EnemyModel enemy)
    {
        Enemy = enemy;
        ID = enemy.ID;
        Name = enemy.Name;
        MaxHP = enemy.HP;
        STR = enemy.STR;
        CON = enemy.CON;
        INT = enemy.INT;
        MEN = enemy.MEN;
        DEX = enemy.DEX;
        AGI = enemy.AGI;
        MOV = enemy.MOV;
        UP = enemy.UP;
        DOWN = enemy.DOWN;
        WT = enemy.WT;
        Controller = enemy.Controller;
        Faction = FactionEnum.Enemy;

        IsAuto = true;
        CurrentHP = MaxHP;
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

        for (int i=0; i<StatusList.Count; i++) 
        {
            if(StatusList[i] is Sleep) 
            {
                StatusList.RemoveAt(i);
                i--;
            }
        }
    }

    public void SetRecover(int recover) 
    {
        CurrentHP += recover;
        if(CurrentHP> MaxHP) 
        {
            CurrentHP = MaxHP;
        }
    }

    public void AddStatus(Status status) 
    {
        StatusList.Add(status);
    }

    public void CheckStatus(List<FloatingNumberData> floatingList) 
    {
        for (int i=0; i<StatusList.Count; i++) 
        {
            if(StatusList[i] is Poison) 
            {
                int damage = ((Poison)StatusList[i]).GetDamage(this);
                FloatingNumberData floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Poison, damage.ToString());
                floatingList.Add(floatingNumberData);
                SetDamage(damage);
            }

            StatusList[i].RemainTime--;
            if(StatusList[i].RemainTime == 0) 
            {
                StatusList.RemoveAt(i);
                i--;
            }
        }
    }

    public void CheckSP() 
    {
        if(CurrentSP < 5) 
        {
            CurrentSP++;
        }
    }
}
