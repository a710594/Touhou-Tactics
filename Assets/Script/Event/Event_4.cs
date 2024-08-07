using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_4 : MyEvent
{
    public override void Start()
    {
        if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.FloorBoss])
        {
            InputMamager.Instance.IsLock = true;
            ConversationUI.Open(6, ()=> 
            {
                InputMamager.Instance.IsLock = false;
                FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.FloorBoss] = true;
            });
            //TutorialUI.Open("場景中有各種可互動的物件。例如前方的那個東西是符卡，靠近後按 space 撿取", "Floor_1", ()=> 
            //{
            //    InputMamager.Instance.IsLock = false;
            //});
            //FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.GetItem] = true;
        }
    }
}
