using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_10 : MyEvent
{
    private CampUI _campUI;

    public override void Start()
    {
        _campUI = GameObject.Find("CampUI").GetComponent<CampUI>();
        _campUI.CookHandler = CookTutorial;
        _campUI.ShopButton.enabled = false;
        _campUI.CookButton.enabled = true;
        _campUI.ExploreButton.enabled = false;
        Vector3 offset = new Vector3(-150, 0, 0);
        TutorialArrowUI.Open("選擇製作料理。", _campUI.CookButton.transform, offset, Vector2Int.right);
        FileSystem.Instance.Save();

    }

    private void CookTutorial() 
    {
        _campUI.CookHandler = null;
        TutorialArrowUI.Close();
        InputMamager.Instance.IsLock = true;
        ConversationUI.Open(12, () =>
        {
            InputMamager.Instance.IsLock = false;
            FlowController.Instance.Info.CurrentStep++;
            FlowController.Instance.Info.LockDic[FlowInfo.LockEnum.Cook] = false;
            _campUI.ShopButton.enabled = true;
            _campUI.ExploreButton.enabled = true;
        });
    }
}
