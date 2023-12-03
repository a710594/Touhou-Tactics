using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//在商店購買的裝備數值會是標準值
//但是從寶箱開出來的裝備數值會是標準值的50%~200%
public class Equip
{
    public EquipModel.CategoryEnum Category;
    public int ID;
    public string Name;
    public string Comment;
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
    public int Price;

    public Equip() { }

    public Equip(EquipModel.CategoryEnum category) 
    {
        Category = category;
        ID = 0;
        Name = "無";
        Comment = "";
        HP = 0;
        ATK = 0;
        DEF = 0;
        MTK = 0;
        MEF = 0;
        DEX = 0;
        AGI = 0;
        MOV = 0;
        Weight = -1;
        Price = 0;
    }

    public Equip(ItemModel item, EquipModel equip) 
    {
        Category = equip.Category;
        ID = equip.ID;
        Name = item.Name;
        Comment = item.Comment;
        HP = equip.HP;
        ATK = equip.ATK;
        DEF = equip.DEF;
        MTK = equip.MTK;
        MEF = equip.MEF;
        DEX = equip.DEX;
        AGI = equip.AGI;
        MOV = equip.MOV;
        Weight = equip.Weight;
        Price = item.Price;
    }

    public Equip(int id)
    {
        ItemModel item = DataContext.Instance.ItemDic[id];
        EquipModel equip = DataContext.Instance.EquipDic[id];

        Category = equip.Category;
        ID = equip.ID;
        Name = item.Name;
        Comment = item.Comment;
        HP = equip.HP;
        ATK = equip.ATK;
        DEF = equip.DEF;
        MTK = equip.MTK;
        MEF = equip.MEF;
        DEX = equip.DEX;
        AGI = equip.AGI;
        MOV = equip.MOV;
        Weight = equip.Weight;
        Price = item.Price;
    }
}
