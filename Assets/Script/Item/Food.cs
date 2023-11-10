using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food
{
    public int ID;
    public string Name;
    public string Comment;
    public int HP = 0;
    public int ATK = 0;
    public int DEF = 0;
    public int MTK = 0;
    public int MEF = 0;
    public int SEN = 0;
    public int AGI = 0;
    public int MOV = 0;
    public int Time = 0;
    public int Price;

    [NonSerialized]
    public Effect Effect;

    public Food() { }

    public Food(ItemModel item, FoodModel food, List<CookAddModel> addList)
    {
        ID = item.ID;
        Name = item.Name;
        Comment = item.Comment + "\n";
        HP = food.HP;
        ATK = food.ATK;
        DEF = food.DEF;
        MTK = food.MTK;
        MEF = food.MEF;
        SEN = food.SEN;
        AGI = food.AGI;
        MOV = food.MOV;
        Time = food.Time;
        Price = item.Price;

        for (int i=0; i<addList.Count; i++) 
        {
            HP += addList[i].HP;
            ATK += addList[i].ATK;
            DEF += addList[i].DEF;
            MTK += addList[i].MTK;
            MEF += addList[i].MEF;
            SEN += addList[i].SEN;
            AGI += addList[i].AGI;
            MOV += addList[i].MOV;
        }

        if (HP > 0)
        {
            Comment += "HP" + HP + " ";
        }
        else if (ATK > 0)
        {
            Comment += "ATK" + ATK + " ";
        }
        else if (DEF > 0)
        {
            Comment += "DEF" + DEF + " ";
        }
        else if (MTK > 0)
        {
            Comment += "MTK" + MTK + " ";
        }
        else if (MEF > 0)
        {
            Comment += "MEF" + MEF + " ";
        }
        else if (SEN > 0)
        {
            Comment += "SEN" + SEN + " ";
        }
        else if (AGI > 0)
        {
            Comment += "AGI" + AGI + " ";
        }
        else if (MOV > 0)
        {
            Comment += "MOV" + MOV + " ";
        }

        SetEffect();
    }

    public void Init()
    {
        SetEffect();
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
        else if (ATK > 0) 
        {
            effect = new BuffEffect(StatusModel.TypeEnum.ATK, ATK, Time);
            effectList.Add(effect);
        }
        else if (DEF > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.DEF, DEF, Time);
            effectList.Add(effect);
        }
        else if (MTK > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.MTK, MTK, Time);
            effectList.Add(effect);
        }
        else if (MEF > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.MEF, MEF, Time);
            effectList.Add(effect);
        }
        else if (SEN > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.SEN, SEN, Time);
            effectList.Add(effect);
        }
        else if (AGI > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.AGI, AGI, Time);
            effectList.Add(effect);
        }
        else if (MOV > 0)
        {
            effect = new BuffEffect(StatusModel.TypeEnum.MOV, MOV, Time);
            effectList.Add(effect);
        }

        Effect = effectList[0];
        Effect.Range = 1;
        Effect.Area = "(0,0)";
        Effect.AreaList = new List<Vector2Int>() { new Vector2Int(0, 0) };
        effect = Effect;
        for (int i=1; i<effectList.Count; i++) 
        {
            effect.SubEffect = effectList[i];
            effect = effect.SubEffect;
            effect.Range = -1;
            effect.Area = "-1";
            effect.AreaList = new List<Vector2Int>();
        }
    }
}
