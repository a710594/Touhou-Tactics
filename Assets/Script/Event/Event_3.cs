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
            TutorialUI.Open("�a�Ϥ����⪺�y��N��Ӽh BOSS�A���|�u�@�q���U�@�h�Ӫ��ӱ�C���˥��N��e���U�@�h�ӡC", "Floor_BOSS", () =>
            {
                InputMamager.Instance.IsLock = false;
            });
            FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.FloorBoss] = true;
        }
    }
}
