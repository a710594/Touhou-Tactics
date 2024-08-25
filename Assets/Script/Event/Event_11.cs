using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_11 : MyEvent
{
    public override void Start()
    {
        if (FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.UseItem_1)
        {
            InputMamager.Instance.IsLock = true;
            ConversationUI.Open(13, true, () =>
            {
                InputMamager.Instance.IsLock = false;
                FlowController.Instance.Info.CurrentStep++;
            }, null);
        }
    }
}
