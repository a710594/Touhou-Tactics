using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent_2 : MyEvent
{
    public override void Start()
    {
        Explore.ExploreManager.Instance.Player.Enable = false;
        ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
        exploreUI.SetVisible(false);
        ConversationUI.Open(2, true, () =>
        {
            Explore.ExploreManager.Instance.Player.Enable = true;
            exploreUI.SetVisible(true);
        });
    }
}
