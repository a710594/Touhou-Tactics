using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EnemyModel
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
    public int MOV; //move 移動
    public int UP;
    public int DOWN;
    public int WT;
    public string Controller;
    public string Sprite_1;
    public string Sprite_2;
    public int Drop_1;
    public int Drop_2;
    public string AI;
    public int Skill_1;
    public int Skill_2;
    public List<string> SpriteList = new List<string>();
    public List<int> DropList = new List<int>();
    public List<int> SkillList = new List<int>();

    public void GetSpriteList()
    {
        if (Sprite_1 != "x")
        {
            SpriteList.Add(Sprite_1);
        }
        if (Sprite_2 != "x")
        {
            SpriteList.Add(Sprite_2);
        }
    }

    public void GetDropList()
    {
        if(Drop_1 != -1) 
        {
            DropList.Add(Drop_1);
        }
        if (Drop_2 != -1)
        {
            DropList.Add(Drop_2);
        }
    }

    public void GetSkillList()
    {
        if (Skill_1 != -1)
        {
            SkillList.Add(Skill_1);
        }
        if (Skill_2 != -1)
        {
            SkillList.Add(Skill_2);
        }
    }
}