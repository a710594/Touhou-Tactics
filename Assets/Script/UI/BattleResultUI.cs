using System;
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

    private Action _callback;

    public void SetWin(int lv, int exp, int addExp, List<int> itemList, Action callback) 
    {
        TitleLabel.text = "You Win!";
        LvLabel.text = "Lv." + lv;
        SetExpBar(lv, exp, addExp);
        List<object> list = new List<object>();
        for (int i=0; i<itemList.Count; i++) 
        {
            list.Add(itemList[i]);
        }
        ScrollView.SetData(list);
        _callback = callback;
    }

    public void SetLose(Action callback) 
    {
        TitleLabel.text = "You Lose...";
        LvLabel.gameObject.SetActive(false);
        ExpBar.gameObject.SetActive(false);
        ScrollView.transform.parent.gameObject.SetActive(false);
        _callback = callback;
    }

    private void SetExpBar(int lv, int exp, int addExp) 
    {
        int needExp = CharacterManager.Instance.NeedExp(lv);
        if (addExp + exp >= needExp)
        {
            ExpBar.SetValueTween(exp, needExp, needExp, ()=> 
            {
                addExp -= (needExp - exp);
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
        _callback();
    }

    private void Awake()
    {
        ConfirmButton.onClick.AddListener(ConfirmOnClick);
    }
}
