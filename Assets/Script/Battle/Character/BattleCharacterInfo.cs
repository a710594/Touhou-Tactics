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
    public int ActionCount = 2; //�C�Ө��ⳣ���⦸����ʾ��|

    public bool HasUseSkill = false;
    public bool HasUseSupport = false;
    public bool HasUseItem = false;
    public bool HasMove = false;
    public bool HasUseSpell = false; //�Ӧ^�X�O�_�w�ιL�ťd

    public bool CanUseSpell = true; //�O�_��ϥβťd

    public string Name;
    public string FileName;
    public int Lv;
    public int MaxHP;
    public int CurrentHP;
    public int STR; //Strength �O�q �v�T���z�ˮ`
    public int CON; //Constitution ��� ��ܪ��z�ˮ`
    public int INT; //Intelligence ���O �v�T�k�N�ˮ`
    public int MEN; //mentality �믫 ��ܪk�N�ˮ`
    public int DEX; //Dexterity �F�� �v�T�R���v
    public int AGI; //Agility �ӱ� ��ܩR���v
    public int MOV; //move ����
    public int WT;
    public int CurrentWT;
    public FactionEnum Faction;
    public Command SelectedCommand = null; //�i��O�ޯ�,���U,�ιD��(���ӫ~,�Ʋz,�ťd)

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
