using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_1 : MyEvent
{
    public override void Start()
    {
        if(FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.BasicOperations)
        //if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BasicOperations])
        {
            InputMamager.Instance.IsLock = true;
            ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
            exploreUI.SetVisible(false);
            ConversationUI.Open(1, true, ()=> 
            {
                InputMamager.Instance.IsLock = false;
                //FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BasicOperations] = true;
                FlowController.Instance.Info.CurrentStep++;
                exploreUI.SetVisible(true);
            }, null);
        }
    }
}
