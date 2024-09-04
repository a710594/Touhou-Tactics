using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Event_2 : MyEvent
{
    public override void Start()
    {
        if(FlowController.Instance.Info.CurrentStep == FlowInfo.StepEnum.FirstBattle)
        {
            InputMamager.Instance.IsLock = true;
            ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
            exploreUI.SetVisible(false);
            ConversationUI.Open(2, true, () =>
            {
                InputMamager.Instance.IsLock = false;
                FlowController.Instance.Info.CurrentStep++;
                exploreUI.SetVisible(true);
            }, null);
        }
    }
}*/
