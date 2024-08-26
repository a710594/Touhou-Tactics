using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_12 : MyEvent
{
    public override void Start()
    {
        if (FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.AddRemainCharacter)
        {
            InputMamager.Instance.IsLock = true;
            ConversationUI.Open(14, true, () =>
            {
                InputMamager.Instance.IsLock = false;
                FlowController.Instance.Info.CurrentStep++;
            }, null);
        }
    }
}
