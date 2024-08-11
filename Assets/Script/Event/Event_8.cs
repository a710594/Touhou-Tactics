using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_8 : MyEvent
{
    public override void Start()
    {
        if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BackCamp])
        {
            InputMamager.Instance.IsLock = true;
            ConversationUI.Open(11, () =>
            {
                InputMamager.Instance.IsLock = false;
                FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BackCamp] = true;
            });
        }
    }
}
