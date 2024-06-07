using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Food : Command
{
    public int ID;
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

    public Food(ItemModel item, FoodResult food, List<int> materialList)
    {
        ID = item.ID;
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
        
        Hit = 100;
        Range = 1;
        CastTarget = TargetEnum.Us;
        EffectTarget = TargetEnum.Us;
        Track = TrackEnum.None;
        AreaList = new List<Vector2Int>(){Vector2Int.zero};

        FoodMaterial foodMaterial;
        for (int i=0; i<materialList.Count; i++) 
        {
            foodMaterial = DataContext.Instance.FoodMaterialDic[materialList[i]];
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
        SetEffect();
    }

    //for debug
    public Food(int id) 
    {
        ItemModel item = DataContext.Instance.ItemDic[id];
        FoodResult food = DataContext.Instance.FoodResultDic[id];

        ID = item.ID;
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

        Hit = 100;
        Range = 1;
        CastTarget = TargetEnum.Us;
        EffectTarget = TargetEnum.Us;
        Track = TrackEnum.None;
        AreaList = new List<Vector2Int>(){Vector2Int.zero};

        SetComment();
        SetEffect();

    }

    public void Init()
    {
        SetEffect();
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

    private void SetEffect() 
    {
        Effect effect;
        List<Effect> effectList = new List<Effect>();
        if (HP > 0) 
        {
            effect = new MedicineEffect(HP);
            effectList.Add(effect);
        }
        if (STR > 0) 
        {
            effect = new BuffEffect(StatusModel.TypeEnum.STR, STR + 100, Time);
            effectList.Add(effect);
        }
        if (CON > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.CON, CON + 100, Time);
            effectList.Add(effect);
        }
        if (INT > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.INT, INT + 100, Time);
            effectList.Add(effect);
        }
        if (MEN > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.MEN, MEN + 100, Time);
            effectList.Add(effect);
        }
        if (SEN > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.SEN, SEN + 100, Time);
            effectList.Add(effect);
        }
        if (AGI > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.AGI, AGI + 100, Time);
            effectList.Add(effect);
        }
        if (MOV > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.MOV, MOV, Time);
            effectList.Add(effect);
        }

        Effect = effectList[0];
        effect = Effect;
        for (int i=1; i<effectList.Count; i++) 
        {
            effect.SubEffect = effectList[i];
            effect = effect.SubEffect;
        }
    }
}
