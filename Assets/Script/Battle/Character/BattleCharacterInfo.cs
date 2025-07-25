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
    //public int ActionCount = 2; //每個角色都有兩次的行動機會

    public bool HasMove = false;
    public bool MoveAgain = false;
    public bool HasSub = false;
    public bool HasMain = false;
    public bool HasSpell = false;
    public bool HasItem = false;

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
    public int CurrentMOV;
    public int WT;
    public int CurrentWT;
    public FactionEnum Faction;
    public Command SelectedCommand = null; //可能是技能,輔助,或道具(消耗品,料理,符卡)

    public List<Skill> SkillList = new List<Skill>();
    public List<Sub> SubList = new List<Sub>();
    public List<Spell> SpellList = new List<Spell>();
    public List<Passive> PassiveList = new List<Passive>();

    public Equip Weapon;
    public List<Equip> Armor = new List<Equip>();
    public List<Equip> Decoration = new List<Equip>();

    public List<Status> StatusList = new List<Status>();

    public List<Vector2Int> StepList = new List<Vector2Int>();
    public List<Vector2Int> RangeList = new List<Vector2Int>();

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

    public List<FloatingNumberData> CheckStatus()
    {
        List<FloatingNumberData> list = new List<FloatingNumberData>();
        for (int i = 0; i < StatusList.Count; i++)
        {
            if (StatusList[i] is Poison)
            {
                int damage = ((Poison)StatusList[i]).GetDamage(this);
                list.Add(new FloatingNumberData(damage.ToString(), EffectModel.TypeEnum.Poison, HitType.Hit));
                SetDamage(damage);
            }

            StatusList[i].RemainTime--;
            if (StatusList[i].RemainTime == 0)
            {
                StatusList.RemoveAt(i);
                i--;
            }
        }
        return list;
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

    public BattleCharacterController GetProvocativeTarget() 
    {
        for (int i = 0; i < StatusList.Count; i++)
        {
            if (StatusList[i] is Provocative)
            {
                return ((Provocative)StatusList[i]).Target;
            }
        }
        return null;
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
