using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent_1 : MyEvent
{
    public override void Start()
    {
        InputMamager.Instance.IsLock = true;
        ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
        exploreUI.SetVisible(false);
        ConversationUI.Open(1, true, ()=> 
        {
            InputMamager.Instance.IsLock = false;;
            exploreUI.SetVisible(true);
        }, null);
    }
}
