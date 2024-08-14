using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowInfo
{
    public enum StepEnum
    {
        None = -1,
        BasicOperations = 0, //基本操作
        FirstBattle,
        SecondBattle,
        ThirdBattle,
        Camp, //第一次到達營地時的對話
        GetItem, //蒐集道具的教學
        FourthBattle,
        BackCamp, //提前返回營地的教學
    }

    public StepEnum CurrentStep;

    public enum LockEnum
    {
        BackCamp,
        Cook,
        Shop,
    }

    public Dictionary<LockEnum, bool> LockDic;

    public void Init() 
    {
        CurrentStep = StepEnum.BasicOperations;
        LockDic = new Dictionary<LockEnum, bool>();
        foreach (LockEnum lockEnum in (LockEnum[])Enum.GetValues(typeof(LockEnum)))
        {
            LockDic.Add(lockEnum, true);
        }
    }
}
