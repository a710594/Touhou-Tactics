using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_6 : MyEvent
{
    public override void Start()
    {
        if(FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.Camp)
        //if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.Camp])
        {
            InputMamager.Instance.IsLock = true;
            ConversationUI.Open(8, () =>
            {
                InputMamager.Instance.IsLock = false;
                //FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.Camp] = true;
                FlowController.Instance.Info.CurrentStep++;
            });
        }
    }
}
