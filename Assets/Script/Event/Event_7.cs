using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_7 : MyEvent
{
    public override void Start()
    {
        Debug.Log(FlowController.Instance.Info.CurrentStep);
        if(FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.GetItem)
        //if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.GetItem])
        {
            InputMamager.Instance.IsLock = true;
            ConversationUI.Open(9, () =>
            {
                InputMamager.Instance.IsLock = false;
                //FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.GetItem] = true;
                FlowController.Instance.Info.CurrentStep++;
            });
        }
    }
}
