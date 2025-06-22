using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaJoinEvent : MyEvent
{
    private CampUI _campUI;

    public override void Start()
    {
        _campUI = GameObject.Find("CampUI").GetComponent<CampUI>();
        _campUI.MainGroup.SetActive(false);
        ConversationUI.Open(6, true, () =>
        {
            CharacterManager.Instance.Info.CharacterList.Add(new CharacterInfo(DataTable.Instance.JobDic[2]));
            ItemManager.Instance.AddItem(7, 5);
            _campUI.MainGroup.SetActive(true);
            _campUI.CookHandler = CookTutorial;
            _campUI.CookButton.gameObject.SetActive(true);
            _campUI.ExploreButton.enabled = false;
            Vector3 offset = new Vector3(-150, 0, 0);
            CampUI campUI = GameObject.Find("CampUI").GetComponent<CampUI>();
            campUI.SetCookButton(true);
            TutorialArrowUI.Open("選擇製作料理。", _campUI.CookButton.transform, offset, Vector2Int.right);
        });
    }

    private void CookTutorial()
    {
        _campUI.CookHandler = null;
        TutorialArrowUI.Close();
        TutorialUI.Open(11, ()=> 
        {
            _campUI.ShopButton.enabled = true;
            _campUI.ExploreButton.enabled = true;
        });
    }
}
