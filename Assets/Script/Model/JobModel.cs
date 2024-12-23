using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class JobModel
{
    public int ID;
    public string Name;
    public int HP;
    public int STR; //Strength 力量 影響物理傷害
    public int CON; //Constitution 體質 抵抗物理傷害
    public int INT; //Intelligence 智力 影響法術傷害
    public int MEN; //mentality 精神 抵抗法術傷害
    public int DEX; //Dexterity 靈巧 影響命中率
    public int AGI; //Agility 敏捷 對抗命中率
    public int MOV;
    public int UP;
    public int DOWN;
    public int WT;
    public int Passive;
    public string Controller;
    public int Skill_1;
    public int Skill_2;
    public int Skill_3;
    public int Skill_4;
    public int Skill_5;
    public int Support_1;
    public int Support_2;
    public int Support_3;
    public int Support_4;
    public int Support_5;
    public int Spell_1;
    public int Weight; //負重,和可穿戴的裝備有關
    public int Armor; //可穿戴的防具數量
    public int Amulets; //可穿戴的護符數量
}