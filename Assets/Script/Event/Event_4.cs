using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_4 : MyEvent
{
    public override void Start()
    {
        if(FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.ThirdBattle)
        //if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.FloorBoss])
        {
            InputMamager.Instance.IsLock = true;
            ConversationUI.Open(6, ()=> 
            {
                InputMamager.Instance.IsLock = false;
                FlowController.Instance.Info.CurrentStep++;
            });
        }
    }
}
