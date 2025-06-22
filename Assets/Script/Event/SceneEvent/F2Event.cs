using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F2Event : MyEvent
{
    public override void Start()
    {
        Explore.ExploreManager.Instance.Player.Enable = false;
        ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
        exploreUI.SetVisible(false);
        ConversationUI.Open(5, true, () =>
        {
            Explore.ExploreManager.Instance.Player.Enable = true;
            exploreUI.SetVisible(true);
        });
    }
}
