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
            TutorialUI.Open("�a�Ϥ����⪺�y��N��ĤH�A�I�쥦�N�|�i�J�԰�", "Floor_1", ()=> 
            {
                InputMamager.Instance.IsLock = false;
            });
            FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.FirstEnemy] = true;
        }
    }
}
