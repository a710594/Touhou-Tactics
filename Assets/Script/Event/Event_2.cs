using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_2 : MyEvent
{
    public override void Start()
    {
        if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.FirstEnemy])
        {
            InputMamager.Instance.IsLock = true;
            //TutorialUI.Open("地圖中黃色的球體代表敵人，碰到它就會進入戰鬥", "Floor_1", ()=> 
            //{
            //    InputMamager.Instance.IsLock = false;
            //});
            //FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.FirstEnemy] = true;
            ConversationUI.Open(2, () =>
            {
                InputMamager.Instance.IsLock = false;
                FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.FirstEnemy] = true;
            });
        }
    }
}
