using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaJoinEvent : MyEvent
{
    private CampUI _campUI;

    public override void Start()
    {
        InputMamager.Instance.IsLock = true;
        ConversationUI.Open(11, true, () =>
        {
            CharacterManager.Instance.Info.CharacterList.Add(new CharacterInfo(DataTable.Instance.JobDic[2]));
            ItemManager.Instance.AddItem(7, 5);
            _campUI = GameObject.Find("CampUI").GetComponent<CampUI>();
            _campUI.CookHandler = CookTutorial;
            _campUI.ShopButton.enabled = false;
            _campUI.CookButton.enabled = true;
            _campUI.ExploreButton.enabled = false;
            Vector3 offset = new Vector3(-150, 0, 0);
            CampUI campUI = GameObject.Find("CampUI").GetComponent<CampUI>();
            campUI.SetCookButton(true);
            TutorialArrowUI.Open("選擇製作料理。", _campUI.CookButton.transform, offset, Vector2Int.right);
        }, null);
    }

    private void CookTutorial()
    {
        _campUI.CookHandler = null;
        TutorialArrowUI.Close();
        InputMamager.Instance.IsLock = true;
        ConversationUI.Open(12, true, () =>
        {
            InputMamager.Instance.IsLock = false;
            _campUI.ShopButton.enabled = true;
            _campUI.ExploreButton.enabled = true;
        }, null);
    }
}
