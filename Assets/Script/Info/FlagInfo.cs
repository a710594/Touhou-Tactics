using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagInfo
{
    public enum FlagEnum 
    {
        BasicOperations, //基本操作
        FirstEnemy, //Floor_1 第一隻怪的說明
        FloorBoss, //樓層Boss的說明
        GetItem, //蒐集道具的教學
        BeforeSpellTutorial, //符卡戰鬥教學之前的對話
        Camp, //第一次到達營地時的對話
        BackCamp, //返回營地的教學
        BackCampBlock, //玩家必需返回營地
    }

    public Dictionary<FlagEnum, bool> FlagDic;

    public void Init() 
    {
        FlagDic = new Dictionary<FlagEnum, bool>();
        foreach (FlagEnum flag in (FlagEnum[])Enum.GetValues(typeof(FlagEnum)))
        {
            FlagDic.Add(flag, false);
        }
    }
}
