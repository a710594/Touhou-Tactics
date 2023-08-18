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
}