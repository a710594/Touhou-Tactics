using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_1 : MyEvent
{
    public override void Start()
    {
        if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BasicOperations])
        {
            InputMamager.Instance.IsLock = true;
            TutorialUI.Open("�򥻾ާ@\nW/�W�G�V�e����\nS/�U�G�V�U����\nA/���G�V������\nD/�k�G�V�k����\nQ�G�V������\nE�G�V�k����", () =>
            {
                InputMamager.Instance.IsLock = false;
            });
            FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BasicOperations] = true;
        }
    }
}
