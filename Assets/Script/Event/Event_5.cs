using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_5 : MyEvent
{
    public override void Start()
    {
        if(FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.SecondBattle)
        //if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BeforeSpellTutorial])
        {
            InputMamager.Instance.IsLock = true;
            ConversationUI.Open(4, ()=> 
            {
                InputMamager.Instance.IsLock = false;
                //FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BeforeSpellTutorial] = true;
                FlowController.Instance.Info.CurrentStep++;
                ItemManager.Instance.AddItem(13, 1);
            });
        }
    }
}
