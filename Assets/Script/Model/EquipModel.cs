using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipModel
{
    public enum CategoryEnum 
    {
        Weapon = 1,
        Armor,
        Amulet
    }

    public CategoryEnum Category;
    public int ID;
    public int Weight;
    public int ATK;
    public int DEF;
    public int MTK;
    public int MEF;
    public int HP;
    public int STR; //Strength 力量 影響物理傷害
    public int CON; //Constitution 體質 抵抗物理傷害
    public int INT; //Intelligence 智力 影響法術傷害
    public int MEN; //mentality 精神 抵抗法術傷害
    public int DEX; //Dexterity 靈巧 影響命中率
    public int AGI; //Agility 敏捷 對抗命中率
    public int MOV; //move 移動
}
