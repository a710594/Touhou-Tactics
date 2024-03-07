using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagInfo
{
    public enum FlagEnum 
    {
        BasicOperations, //�򥻾ާ@
        FirstEnemy, //Floor_1 �Ĥ@���Ǫ�����
        FloorBoss, //�ӼhBoss������
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
