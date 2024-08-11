using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Event_9 : MyEvent
{
    public Event_9() 
    {
        SceneController.Instance.AfterSceneLoadedHandler += AfterBackCamp;
    }

    public override void Start()
    {
        if (!FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BackCampBlock])
        {
            InputMamager.Instance.IsLock = true;
            ConfirmUI.Open("請先按 Esc 返回營地", "確定", ()=>
            {
                Vector3 moveTo = new Vector3(2, 1, 19);
                Explore.ExploreManager.Instance.Player.MoveTo = moveTo;
                Explore.ExploreManager.Instance.Player.transform.DOMove(moveTo, 1).OnComplete(()=> 
                {
                    InputMamager.Instance.IsLock = false;

                });
            });
        }
    }

    private void AfterBackCamp(string sceneName) 
    {
        if (sceneName == "Camp")
        {
            FlagManager.Instance.Info.FlagDic[FlagInfo.FlagEnum.BackCampBlock] = true;
            SceneController.Instance.AfterSceneLoadedHandler -= AfterBackCamp;
        }
    }
}
