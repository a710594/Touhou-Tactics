using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Event_9 : MyEvent
{
    public override void Start()
    {
        if(FlowController.Instance.Info.LockDic[FlowInfo.LockEnum.BackCamp])
        //if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BackCampBlock])
        {
            InputMamager.Instance.IsLock = true;
            ConfirmUI.Open("按 Esc 選擇返回營地", "確定", ()=>
            {
                Vector3 moveTo = new Vector3(2, 1, 19);
                Explore.ExploreManager.Instance.Player.MoveTo = moveTo;
                Explore.ExploreManager.Instance.Player.transform.DOMove(moveTo, 1).OnComplete(()=> 
                {
                    InputMamager.Instance.IsLock = false;
                    //FlowController.Instance.Info.CurrentStep++;
                    //FlowController.Instance.Info.LockDic[FlowInfo.LockEnum.BackCamp] = false;
                });
            });
        }
    }
}
