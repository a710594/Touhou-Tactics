using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacterInfo
{
    public static readonly Vector3 DefaultLastPosition = new(int.MaxValue, int.MaxValue, int.MaxValue);

    public enum FactionEnum
    {
        Player,
        Enemy,
        None,
    }

    public bool IsAuto = false;
    public int ActionCount = 2; //每個角色都有兩次的行動機會

    public bool HasUseSkill = false;
    public bool HasUseSupport = false;
    public bool HasUseItem = false;
    public bool HasMove = false;
    public bool HasUseSpell = false; //該回合是否已用過符卡

    public bool CanUseSpell = true; //是否能使用符卡

    public string Name;
    public string FileName;
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
    public int WT;
    public int CurrentWT;
    public FactionEnum Faction;
    public Command SelectedCommand = null; //可能是技能,輔助,或道具(消耗品,料理,符卡)

    public List<Skill> SkillList = new List<Skill>();
    public List<Support> SupportList = new List<Support>();
    public List<Spell> SpellList = new List<Spell>();
    public List<Passive> PassiveList = new List<Passive>();

    public Equip Weapon;
    public List<Equip> Armor = new List<Equip>();
    public List<Equip> Decoration = new List<Equip>();

    public List<Status> StatusList = new List<Status>();

    public BattleCharacterInfo() { }

    public void SetDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP < 0)
        {
            CurrentHP = 0;
        }

        for (int i = 0; i < StatusList.Count; i++)
        {
            if (StatusList[i] is Sleep)
            {
                StatusList.RemoveAt(i);
                i--;
            }
        }
    }

    public void AddStatus(Status status)
    {
        StatusList.Add((Status)status.Clone()); ;
    }

    public List<Log> CheckStatus()
    {
        List<Log> logList = new List<Log>();
        for (int i = 0; i < StatusList.Count; i++)
        {
            if (StatusList[i] is Poison)
            {
                int damage = ((Poison)StatusList[i]).GetDamage(this);
                Log log = new Log(this, damage.ToString());
                logList.Add(log);
                SetDamage(damage);
            }

            StatusList[i].RemainTime--;
            if (StatusList[i].RemainTime == 0)
            {
                StatusList.RemoveAt(i);
                i--;
            }
        }
        return logList;
    }

    public bool IsSleep()
    {
        for (int i = 0; i < StatusList.Count; i++)
        {
            if (StatusList[i] is Sleep)
            {
                return true;
            }
        }
        return false;
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
