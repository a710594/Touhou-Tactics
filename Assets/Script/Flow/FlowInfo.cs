using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowInfo
{
    public enum LockEnum
    {
        BackCamp,
        Cook,
        Shop,
    }

    //public enum EventConditionEnum
    //{
    //    None,
    //    F1,
    //    F2,
    //    SanaeJoin,
    //    MarisaJoin,
    //    Cook,
    //}

    public bool HasGetItem = false;
    public bool HasSanaeTutorial = false;
    public bool HasGetCard = false;
    public bool HasUseCard = false;
    public int TriggerEvent = 1;
    //public int SceneEvent = 1;

    public List<string> EventConditionList = new List<string>();
    public Dictionary<LockEnum, bool> LockDic;

    public void Init() 
    {
        //CurrentStep = StepEnum.BasicOperations;
        LockDic = new Dictionary<LockEnum, bool>();
        foreach (LockEnum lockEnum in (LockEnum[])Enum.GetValues(typeof(LockEnum)))
        {
            LockDic.Add(lockEnum, true);
        }
    }
}
