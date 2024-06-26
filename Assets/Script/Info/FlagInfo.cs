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
