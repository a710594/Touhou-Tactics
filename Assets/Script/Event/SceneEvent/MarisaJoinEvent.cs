using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaJoinEvent : MyEvent
{
    public override void Start()
    {
        InputMamager.Instance.IsLock = true;
        ConversationUI.Open(14, true, () =>
        {
            CharacterManager.Instance.Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[2]));
            CharacterManager.Instance.Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[3]));
            CharacterManager.Instance.Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[5]));
            CharacterManager.Instance.Info.CharacterList.Add(new CharacterInfo(DataContext.Instance.JobDic[6]));
            InputMamager.Instance.IsLock = false;
        }, null);
    }
}
