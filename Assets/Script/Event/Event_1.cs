using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_1 : MyEvent
{
    public override void Start()
    {
        if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BasicOperations])
        {
            InputMamager.Instance.IsLock = true;
            //TutorialUI.Open("基本操作\nW/上：向前移動\nS/下：向下移動\nA/左：向左旋轉\nD/右：向右旋轉\nQ：向左平移\nE：向右平移", () =>
            //{
            //    InputMamager.Instance.IsLock = false;
            //});
            ConversationUI.Open(1, ()=> 
            {
                InputMamager.Instance.IsLock = false;
                FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BasicOperations] = true;
            });
        }
    }
}
