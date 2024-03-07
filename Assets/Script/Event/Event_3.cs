using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_3 : MyEvent
{
    public override void Start()
    {
        if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.FloorBoss])
        {
            InputMamager.Instance.IsLock = true;
            TutorialUI.Open("瓜い︹瞴砰加糷 BOSSウ穦臔硄┕糷加加辫ゴウ碞玡┕糷加", "Floor_BOSS", () =>
            {
                InputMamager.Instance.IsLock = false;
            });
            FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.FloorBoss] = true;
        }
    }
}
