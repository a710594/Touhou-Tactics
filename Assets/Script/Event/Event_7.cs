using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_7 : MyEvent
{
    public override void Start()
    {
        if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.GetItem])
        {
            InputMamager.Instance.IsLock = true;
            ConversationUI.Open(9, () =>
            {
                InputMamager.Instance.IsLock = false;
                FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.GetItem] = true;
            });
        }
    }
}
