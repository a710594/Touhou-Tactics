using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuJoinEvent : MyEvent
{
    public override void Start()
    {
        CampUI campUI = GameObject.Find("CampUI").GetComponent<CampUI>();
        campUI.MainGroup.SetActive(false);
        InputMamager.Instance.IsLock = true;
        ConversationUI.Open(4, true, () =>
        {
            CharacterManager.Instance.Info.CharacterList.Add(new CharacterInfo(DataTable.Instance.JobDic[1]));
            InputMamager.Instance.IsLock = false;
            TutorialUI.Open(9, ()=> 
            {
                campUI.MainGroup.SetActive(true);
            });
        });
    }
}
