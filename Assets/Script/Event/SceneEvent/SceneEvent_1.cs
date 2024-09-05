using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEvent_1 : MyEvent
{
    public override void Start()
    {
        InputMamager.Instance.IsLock = true;
        ConversationUI.Open(7, true, () =>
        {
            CharacterManager.Instance.Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[7]));
            InputMamager.Instance.IsLock = false;
        }, null);
    }
}
