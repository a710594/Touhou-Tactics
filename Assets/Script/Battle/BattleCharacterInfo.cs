using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle
{
    public class BattleCharacterInfo
    {
        public static readonly int MaxPP = 10;
        public static readonly Vector3 DefaultLastPosition = new(int.MaxValue, int.MaxValue, int.MaxValue);

        public enum FactionEnum
        {
            Player,
            Enemy,
            None,
        }

        //基礎屬性
        public int JobId;
        public string Name;
        public int Lv;
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
        public string Sprite;
        public FactionEnum Faction;
        public JobModel Job = null;
        public EnemyModel Enemy = null;
        public Equip Weapon = new Equip(EquipModel.CategoryEnum.Weapon);
        public Equip Armor = new Equip(EquipModel.CategoryEnum.Armor);
        public Equip[] Amulets = new Equip[2] { new Equip(EquipModel.CategoryEnum.Amulet), new Equip(EquipModel.CategoryEnum.Amulet) };

        public List<Skill> SkillList = new List<Skill>();
        public List<Support> SupportList = new List<Support>();
        public List<Passive> PassiveList = new List<Passive>();
        public List<Spell> CardList = new List<Spell>();

        //當前屬性
        public int Index; //戰鬥的時候用
        public bool IsAuto = false;
        public bool HasUseSkill = false;
        public bool HasUseSupport = false;
        public bool HasUseItem = false;
        public bool HasMove = false;
        public bool HasUseSpell = false;
        public int CurrentHP;
        public int CurrentPP = 2; //符卡點數
        public int CurrentWT;
        public int ActionCount = 2; //每個角色都有兩次的行動機會
        public Vector3 Position = new Vector3();
        public Vector3 LastPosition = new Vector3();
        public Command SelectedCommand = null; //可能是技能,輔助,或道具(消耗品,料理,符卡)
        public BattleAI AI = null;
        public Vector2Int Direction = Vector2Int.left;
        public List<Status> StatusList = new List<Status>();

        public BattleCharacterInfo(int lv, JobModel job)
        {
            Job = job;
            JobId = job.ID;
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
            UP = job.UP;
            DOWN = job.DOWN;
            WT = job.WT;
            if (job.Passive != -1)
            {
                PassiveList.Add(PassiveFactory.GetPassive(job.Passive));
            }
            Controller = job.Controller;
            Sprite = job.Controller;
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
                CardList.Add(new Spell(DataContext.Instance.SpellList[job.Spell_1]));
            }
        }

        public BattleCharacterInfo(int lv, EnemyModel enemy)
        {
            Enemy = enemy;
            Name = enemy.Name;
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
            UP = enemy.UP;
            DOWN = enemy.DOWN;
            WT = enemy.WT;
            Controller = enemy.Controller;
            Sprite = enemy.SpriteList[0];
            Faction = FactionEnum.Enemy;

            for (int i = 0; i < enemy.SkillList.Count; i++)
            {
                SkillList.Add(new Skill(DataContext.Instance.SkillDic[enemy.SkillList[i]]));
            }

            IsAuto = true;
            CurrentHP = MaxHP;
            CurrentWT = WT;
        }

        public BattleCharacterInfo(CharacterInfo info, int lv)
        {
            Name = info.Name;
            Lv = lv;
            Job = DataContext.Instance.JobDic[info.JobId];
            JobId = info.JobId;

            MaxHP = info.MaxHP;
            STR = info.STR;
            CON = info.CON;
            INT = info.INT;
            MEN = info.MEN;
            DEX = info.DEX;
            AGI = info.AGI;
            MOV = info.MOV;
            UP = info.UP;
            DOWN = info.DEX;
            WT = info.WT;
            PassiveList = info.PassiveList;
            Controller = info.Controller;
            Sprite = info.Controller;
            Faction = FactionEnum.Player;

            CurrentHP = info.CurrentHP;
            CurrentWT = WT;

            SkillList = info.SkillList;
            SupportList = info.SupportList;
            CardList = info.CardList;

            Weapon = info.Weapon;
            Armor = info.Armor;
            Amulets = info.Amulets;
        }

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

        public void SetRecover(int recover)
        {
            CurrentHP += recover;
            if (CurrentHP > MaxHP)
            {
                CurrentHP = MaxHP;
            }
        }

        public void AddStatus(Status status)
        {
            StatusList.Add((Status)status.Clone()); ;
        }

        public void CheckStatus(List<FloatingNumberData> floatingList)
        {
            for (int i = 0; i < StatusList.Count; i++)
            {
                if (StatusList[i] is Poison)
                {
                    int damage = ((Poison)StatusList[i]).GetDamage(this);
                    FloatingNumberData floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Poison, damage.ToString());
                    floatingList.Add(floatingNumberData);
                    SetDamage(damage);
                }

                StatusList[i].RemainTime--;
                if (StatusList[i].RemainTime == 0)
                {
                    StatusList.RemoveAt(i);
                    i--;
                }
            }
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

        public void ChangeSprite(string sprite) 
        {
            Sprite = sprite;
        }
    }
}