using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowInfo
{
    //public enum StepEnum
    //{
    //    BasicOperations = 0, //基本操作
    //    FirstBattle = 1,
    //    SecondBattle = 2,
    //    ThirdBattle = 3,
    //    Camp = 4, //第一次到達營地時的對話
    //    GetItem = 5, //蒐集道具的教學
    //    BackCamp = 6, //提前返回營地的教學
    //    Cook = 7,
    //    UseItem_1 = 8,
    //    AddRemainCharacter,
    //    End,
    //}

    //public StepEnum CurrentStep;

    public enum LockEnum
    {
        BackCamp,
        Cook,
        Shop,
    }

    public enum EventConditionEnum
    {
        F1,
        F2,
        SanaeJoin,
        MarisaJoin,
        Cook,
    }

    public bool HasGetItem = false;
    public bool HasSanaeTutorial = false;
    public bool HasGetCard = false;
    public bool HasUseCard = false;
    public int TriggerEvent = 1;
    //public int SceneEvent = 1;

    public List<EventConditionEnum> EventConditionList = new List<EventConditionEnum>();
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
