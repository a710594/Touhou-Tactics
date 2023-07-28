using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattleCharacterInfo
{
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
    public string Controller;
    public FactionEnum Faction;

    public List<Skill> SkillList = new List<Skill>();
    public List<Support> SupportList = new List<Support>();

    //當前屬性
    public bool IsAuto = false;
    public bool HasUseSkill = false;
    public bool HasUseSupport = false;
    public int CurrentHP;
    public int CurrentPP;
    public int CurrentWT;
    public int CurrentSP = 2; //support point
    public int ActionCount = 2; //每個角色都有兩次的行動機會
    public Vector3 Position = new Vector3();
    public Vector3 LastPosition = new Vector3();
    public Skill SelectedSkill = null;
    public Support SelectedSupport = null;
    public Item SelectedItem = null;
    public AI AI = null;
    public List<Status> StatusList = new List<Status>();

    public BattleCharacterInfo(JobModel job) 
    {
        ID = job.ID;
        Name = job.Name;
        MaxHP = job.HP;
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
        ID = enemy.ID;
        Name = enemy.Name;
        MaxHP = enemy.HP;
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
