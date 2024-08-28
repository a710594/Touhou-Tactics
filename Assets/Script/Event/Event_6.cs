using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_6 : MyEvent
{
    public override void Start()
    {
        if(FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.Camp)
        {
            InputMamager.Instance.IsLock = true;
            ConversationUI.Open(8, true, () =>
            {
                InputMamager.Instance.IsLock = false;
                FlowController.Instance.Info.CurrentStep++;
                CharacterManager.Instance.Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[2]));
            }, null);
        }
    }
}
