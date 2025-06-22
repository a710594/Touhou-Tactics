using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : BaseUI
{
    public Action CloseHandler;
    public Action<CharacterScrollItem> DetailHandler = null;
    public Action<CharacterScrollItem> UseItemHandler = null;

    public RectTransform RectTransform;
    public ScrollView ScrollView;
    public TipLabel TipLabel;
    public Button CloseButton;
    public Text LvLabel;
    public Text ExpLabel;

    private CharacterScrollItem _scrollItem;

    public static CharacterUI Open()
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/CharacterUI"), Vector3.zero, Quaternion.identity);
        GameObject canvas = GameObject.Find("Canvas");
        obj.transform.SetParent(canvas.transform);
        CharacterUI characterUI = obj.GetComponent<CharacterUI>();
        characterUI.Init();
        InputMamager.Instance.CurrentUI = characterUI;

        return characterUI;
    }

    public void Init()
    {
        RectTransform.offsetMax = Vector3.zero;
        RectTransform.offsetMin = Vector3.zero;
        LvLabel.text = "隊伍等級：" + CharacterManager.Instance.Info.Lv;
        ExpLabel.text = "經驗值：" + CharacterManager.Instance.Info.Exp + "/" + CharacterManager.Instance.NeedExp(CharacterManager.Instance.Info.Lv);
        ScrollView.SetData(new List<object>(CharacterManager.Instance.Info.CharacterList));
        ScrollView.SetIndex(0);
        for (int i=0; i<ScrollView.GridList.Count; i++) 
        {
            for (int j=0; j<ScrollView.GridList[i].ScrollItemList.Count; j++) 
            {
                ((CharacterScrollItem)ScrollView.GridList[i].ScrollItemList[j]).DetailHandler += DetailOnClick;
                ((CharacterScrollItem)ScrollView.GridList[i].ScrollItemList[j]).UseItemHandler += UseItemOnClick;
            }
        }
    }

    public void Close()
    {
        if (CloseHandler != null) 
        {
            CloseHandler();
        }

        Destroy(gameObject);
    }

    public override void COnClick()
    {
        Close();
    }

    private void DetailOnClick(CharacterScrollItem scrollItem) 
    {
        if (DetailHandler == null)
        {
            CharacterDetailUI characterDetailUI = CharacterDetailUI.Open();
            characterDetailUI.SetData((CharacterInfo)scrollItem.Data);
            characterDetailUI.CloseHandler = () =>
            {
                InputMamager.Instance.CurrentUI = this;
            };
        }
        else
        {
            DetailHandler(scrollItem);
        }
    }

    private void UseItemOnClick(CharacterScrollItem scrollItem) 
    {
        if (UseItemHandler == null)
        {
            _scrollItem = scrollItem;
            BagUI bagUI = BagUI.Open();
            bagUI.SetUseState();
            bagUI.UseHandler += UseItem;
        }
        else
        {
            UseItemHandler(scrollItem);
        }
    }

    private void UseItem(object obj)
    {
        int add = 0;
        if (obj is Battle.ItemCommand)
        {
            Battle.ItemCommand item = (Battle.ItemCommand)obj;
            add = item.Effect.Value;
        }
        else if (obj is Food)
        {
            Food food = (Food)obj;
            add = food.HP;
        }

        CharacterInfo info = (CharacterInfo)_scrollItem.Data;
        info.SetRecover(add);
        _scrollItem.HpBar.SetValueTween(info.CurrentHP, info.MaxHP, null);
        TipLabel.SetLabel(info.Name + " 回復了 " + add + " HP");
    }

    private void Awake()
    {
        CloseButton.onClick.AddListener(Close);
    }
}
