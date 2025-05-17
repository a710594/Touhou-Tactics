using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent_3 : MyEvent
{
    public override void Start()
    {
        InputMamager.Instance.IsLock = true;
        ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
        exploreUI.SetVisible(false);
        ConversationUI.Open(3, true, () =>
        {
            CharacterManager.Instance.Info.CharacterList.Add(new CharacterInfo(DataTable.Instance.JobDic[7]));
            InputMamager.Instance.IsLock = false;
            exploreUI.SetVisible(true);
        });
    }
}
