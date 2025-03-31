using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Food
{
    public string Name;
    public string Comment;
    public int HP = 0;
    public int STR = 0;
    public int CON = 0;
    public int INT = 0;
    public int MEN = 0;
    public int SEN = 0;
    public int AGI = 0;
    public int MOV = 0;
    public int Time = 0;
    public int Price;

    public Food() { }

    public Food(ItemModel item, FoodResultModel food, List<int> materialList)
    {
        Name = item.Name;
        Comment = item.Comment + "\n";
        HP = food.HP;
        STR = food.STR;
        CON = food.CON;
        INT = food.INT;
        MEN = food.MEN;
        SEN = food.SEN;
        AGI = food.AGI;
        MOV = food.MOV;
        Time = food.Time;
        Price = item.Price;

        FoodMaterialModel foodMaterial;
        for (int i=0; i<materialList.Count; i++) 
        {
            foodMaterial = DataTable.Instance.FoodMaterialDic[materialList[i]];
            HP += foodMaterial.HP;
            STR += foodMaterial.STR;
            CON += foodMaterial.CON;
            INT += foodMaterial.INT;
            MEN += foodMaterial.MEN;
            SEN += foodMaterial.SEN;
            AGI += foodMaterial.AGI;
            MOV += foodMaterial.MOV;
        }

        SetComment();
    }

    //for debug
    public Food(int id) 
    {
        ItemModel item = DataTable.Instance.ItemDic[id];
        FoodResultModel food = DataTable.Instance.FoodResultDic[id];

        Name = item.Name;
        Comment = item.Comment + "\n";
        HP = food.HP;
        STR = food.STR;
        CON = food.CON;
        INT = food.INT;
        MEN = food.MEN;
        SEN = food.SEN;
        AGI = food.AGI;
        MOV = food.MOV;
        Time = food.Time;
        Price = item.Price;

        SetComment();

    }

    private void SetComment() 
    {
        if (HP > 0)
        {
            Comment += "HP+" + HP + " ";
        }
        if (STR > 0)
        {
            Comment += "STR+" + STR + "% ";
        }
        if (CON > 0)
        {
            Comment += "CON+" + CON + "% ";
        }
        if (INT > 0)
        {
            Comment += "INT+" + INT + "% ";
        }
        if (MEN > 0)
        {
            Comment += "MEN+" + MEN + "% ";
        }
        if (SEN > 0)
        {
            Comment += "SEN+" + SEN + "% ";
        }
        if (AGI > 0)
        {
            Comment += "AGI+" + AGI + "% ";
        }
        if (MOV > 0)
        {
            Comment += "MOV+" + MOV + " ";
        }
    }
}
