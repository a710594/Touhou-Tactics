using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Explore;

public class BattleResultUI : MonoBehaviour
{
    public Text TitleLabel;
    public Text LvLabel;
    public ValueBar ExpBar;
    public ScrollView ScrollView;
    public Button ConfirmButton;

    public void SetData(int lv, int exp, int addExp, List<int> itemList) 
    {
        LvLabel.text = "Lv." + lv;
        SetExpBar(lv, exp, addExp);
        List<object> list = new List<object>();
        for (int i=0; i<itemList.Count; i++) 
        {
            list.Add(itemList[i]);
        }
        ScrollView.SetData(list);
    }

    public void SetExpBar(int lv, int exp, int addExp) 
    {
        int needExp = (int)Mathf.Pow(lv, 3) - exp;
        if (addExp >= needExp)
        {
            ExpBar.SetValueTween(exp, needExp, needExp, ()=> 
            {
                addExp -= needExp;
                lv++;
                exp = 0;
                LvLabel.text = "Lv." + lv;
                SetExpBar(lv, exp, addExp);
            });
        }
        else
        {
            ExpBar.SetValueTween(exp, exp + addExp, needExp, null);
            exp += addExp;
            addExp = 0;
        }
    }

    private void ConfirmOnClick() 
    {
        SceneController.Instance.ChangeScene("Explore", ()=> 
        {
            ExploreManager.Instance.Reload();
        });
    }

    private void Awake()
    {
        ConfirmButton.onClick.AddListener(ConfirmOnClick);
    }
}
